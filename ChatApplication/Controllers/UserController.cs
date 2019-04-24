#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  17.04.2019 23:09
#endregion

using System.Linq;
using ChatApplication.Poco;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using ChatApplication.Dbl;
using ChatApplication.Dbl.Models;
using Microsoft.Extensions.Logging;

namespace ChatApplication.Controllers
{
    /// <summary>
    /// Контроллер пользователя
    /// </summary>
    [Route("api/v1/user")]
    [Authorize]
    public class UserController : Controller
    {
        /// <summary>
        /// Контекст дб
        /// </summary>
        private DbContext _ctx;
        /// <summary>
        /// Логгер
        /// </summary>
        private ILogger _logger;       
       /// <summary>
       /// Контроллер для отображения пользовательской информации.
       /// </summary>
       /// <param name="ctx">Контекст бд</param>
       /// <param name="logger">Логгер</param>
        public UserController(DbContext ctx, ILogger<UserController> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }
        /// <summary>
        /// Получение залогоненого пользователя
        /// </summary>
        /// <returns>Возвращает основной обьект пользователя, содержит данные о открытых топиках и количестве непрочтенных сообщений.</returns>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var users = await _ctx.Users.GetUsers();
            var user = users.FirstOrDefault(x=>x.UserName == User.Identity.Name);
            var appUser = Mapper.Map<ApplicationUser>(user);
            var topics = await _ctx.Topics.GetByUserId(user.Id);
            appUser.Topics = topics;
            //TODO: удалить фейк
            appUser.NewMessages = 122;
            return Json(appUser);            
        }
    }
}