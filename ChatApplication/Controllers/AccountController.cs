#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  14.04.2019 7:38
#endregion
using ChatApplication.Code;
using ChatApplication.Dbl;
using ChatApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Logging;

namespace ChatApplication.Controllers
{
    /// <summary>
    /// Контроллер для работы с аккаунтами
    /// </summary>
    [Route("api/v1/[controller]")]
    public class AccountController : Controller
    {
        /// <summary>
        /// пользовательский датастор
        /// </summary>
        private DbContext _userDs;
        /// <summary>
        /// Конструктор с пользовательским датасорсом
        /// </summary>
        /// <param name="userDs"></param>
        public AccountController(DbContext userDs)
        {
            _userDs = userDs;
        }
        /// <summary>
        /// Получение токена для приложения
        /// </summary>
        /// <param name="model">Модель входа</param>
        /// <returns></returns>
        [HttpPost("token")]
        public async Task<IActionResult> Token([FromBody]LoginModel model)
        {

            var username = model.UserName;
            var password = model.Password;

            var identity = await GetIdentity(username, password);
            if (identity == null)
            {
                Response.StatusCode = 400;                
                return NotFound("Invalid username or password.");
            }

            var encodedJwt = CreateToken(identity);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };                      
            return Json(response);            
        }

        private string CreateToken(ClaimsIdentity identity)
        {
            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh([FromBody] RefreshTokenModel token)
        {
            var principal = GetPrincipalFromExpiredToken(token.Token);            
            var encodedJwt = CreateToken(principal.Identities.First());
            var response = new
            {
                access_token = encodedJwt,
                username = principal.Identity.Name
            };
            return Json(response);

        }       
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            
            var tokenValidationParameters = new TokenValidationParameters
            {                
                ValidateIssuer = true,
                // строка, представляющая издателя
                ValidIssuer = AuthOptions.ISSUER,

                // будет ли валидироваться потребитель токена
                ValidateAudience = true,
                // установка потребителя токена
                ValidAudience = AuthOptions.AUDIENCE,
                // будет ли валидироваться время существования
                ValidateLifetime = true,

                // установка ключа безопасности
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                // валидация ключа безопасности
                ValidateIssuerSigningKey = true,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            IdentityModelEventSource.ShowPII = true;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
        private async Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            var person = (await _userDs.Users.GetUsers()).First(x => x.UserName == username);
            var role = await _userDs.Roles.GetRolesForUser(person.Id);
            var roleName = role.First().Name;
            if (person == null) return null; // если пользователя не найдено
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, person.UserName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, roleName)
            };
            var claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}
