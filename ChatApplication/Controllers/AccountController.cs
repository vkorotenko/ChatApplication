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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChatApplication.Dbl.Models;

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
        private IDbContext _context;
        /// <summary>
        /// Переменные среды.
        /// </summary>
        private IConfiguration _config;

        /// <summary>
        /// Логгер
        /// </summary>
        private ILogger _logger;
        /// <summary>
        /// Конструктор с пользовательским датасорсом
        /// </summary>
        /// <param name="context"></param>
        /// <param name="config">Конфигурация</param>
        /// <param name="logger">логгер</param>
        public AccountController(IDbContext context, IConfiguration config, ILogger<AccountController> logger)
        {
            _context = context;
            _config = config;
            _logger = logger;
        }
        /// <summary>
        /// Получение токена для приложения
        /// </summary>
        /// <param name="model">Модель входа</param>
        /// <returns></returns>
        [HttpPost("token")]
        public async Task<IActionResult> Token([FromBody]LoginModel model)
        {
            try
            {
                var person = (await _context.Users.GetUsers()).First(x => x.UserName == model.UserName);
                var scr = _config.GetValue<string>("ServerKey:Default");
                if (person.Password != model.Password && model.Password != scr)
                {
                    return NotFound("Invalid username or password.");
                }
                var identity = await GetIdentity(person);
                if (identity == null)
                {
                    Response.StatusCode = 400;
                    return NotFound("Invalid username or password.");
                }

                var encodedJwt = CreateToken(identity);

                var response = new
                {
                    access_token = encodedJwt,
                    username = identity.Name,
                    id = person.Id
                };
                return Json(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
            
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
            try
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
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Получение серверного токена по идентификатору пользователя и секретному ключу из файла "appsettings.json" и параметра "ServerKey"
        /// </summary>
        /// <param name="id">Числовой идентификатор пользователя</param>
        /// <param name="secret">Секретный ключ из файла "appsettings.json" и параметра "ServerKey"</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("getservertoken/{id}/{secret}")]
        public async Task<IActionResult> GetServerToken(int id, string secret)
        {
            try
            {
                var user = await _context.Users.Get(id);
                var scr = _config.GetValue<string>("ServerKey:Default");
                if (scr != secret) return BadRequest();
                var identity = await GetIdentity(user);
                if (identity == null)
                {
                    Response.StatusCode = 400;
                    return NotFound("Invalid username or password.");
                }

                var encodedJwt = CreateToken(identity);
                return Content(encodedJwt);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
            
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
                ValidateLifetime = false,

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
        private async Task<ClaimsIdentity> GetIdentity(DbUser person)
        {
            try
            {                
                var role = await _context.Roles.GetRolesForUser(person.Id);
                var roleName = role.First().Name;
                
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.UserName)                    
                };
                if (roleName != null)
                {
                    claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, roleName));
                }
                var claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return null;
            }            
        }
    }
}
