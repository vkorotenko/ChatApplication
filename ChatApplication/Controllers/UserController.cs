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

namespace ChatApplication.Controllers
{
    /// <summary>
    /// Класс для получения собеседников
    /// </summary>
    [Route("api/v1/[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        /// <summary>
        /// Контекст дб
        /// </summary>
        private DbContext _ctx;
       
       /// <summary>
       /// Контроллер для отображения пользовательской информации.
       /// </summary>
       /// <param name="ctx"></param>
        public UserController(DbContext ctx)
        {
            _ctx = ctx;
        }
        /// <summary>
        /// Получение залогоненого пользователя
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var users = await _ctx.Users.GetUsers();
            var user = users.FirstOrDefault(x=>x.UserName == User.Identity.Name);
            var appUser = Mapper.Map<ApplicationUser>(user);
            var topics = await _ctx.Topics.GetByUserId(user.Id);
            appUser.Topics = topics;
            return Json(appUser);
            //return Ok($"Ваш логин: {User.Identity.Name}");
        }
    }
}