#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  13.04.2019 22:30
#endregion
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplication.Controllers
{
    /// <summary>
    /// Контроллер значений
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ValuesController : Controller
    {
        /// <summary>
        /// Информация о логоне
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("getlogin")]
        [HttpGet]
        public IActionResult GetLogin()
        {
            return Ok($"Ваш логин: {User.Identity.Name}");
        }
        /// <summary>
        /// Получение роли пользователя
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [Route("getrole")]
        [HttpGet]
        public IActionResult GetRole()
        {
            return Ok("Ваша роль: администратор");
        }
    }
}
