#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  14.04.2019 7:38
#endregion
using ChatApplication.Code;
using ChatApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using ChatApplication.Poco;
using ChatApplication.Services;

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
        private IDataStore<ApplicationUser> _userDs;
        /// <summary>
        /// Конструктор с пользовательским датасорсом
        /// </summary>
        /// <param name="userDs"></param>
        public AccountController(IDataStore<ApplicationUser> userDs)
        {
            _userDs = userDs;
        }
        /// <summary>
        /// Получение токена для приложения
        /// </summary>
        /// <param name="model">Модель входа</param>
        /// <returns></returns>
        [HttpPost("token")]
        public async Task Token([FromBody]LoginModel model)
        {

            var email = model.Phone;
            var password = model.Password;

            var identity = await GetIdentity(email, password);
            if (identity == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Invalid email or password.");
                return;
            }

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

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            var co = new CookieOptions
            {
                Domain = Request.Host.Value,
                Expires = jwt.ValidTo,
                HttpOnly = true,
                Path = "/"
            };
            Response.Cookies.Append("Bearer", response.access_token, co);
            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

        private async Task<ClaimsIdentity> GetIdentity(string email, string password)
        {
            var person = await ((IUserDs) _userDs.GetItemsAsync()).GetItemByPhone(email);
            if (person == null) return null; // если пользователя не найдено
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, person.Email),
            };
            var claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}
