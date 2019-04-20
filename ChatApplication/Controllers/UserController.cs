#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  17.04.2019 23:09
#endregion

using ChatApplication.Poco;
using ChatApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        /// пользовательский датастор
        /// </summary>
        private IDataStore<ApplicationUser> _applicationUserDataStore;
        
        /// <summary>
        /// Копаньонский датастор
        /// </summary>
        private IDataStore<Companion> _applicationCompanionDataStore;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationUserDataStore"></param>
        /// <param name="applicationCompanionDataStore"></param>
        public UserController(IDataStore<ApplicationUser> applicationUserDataStore, IDataStore<Companion> applicationCompanionDataStore)
        {
            _applicationCompanionDataStore = applicationCompanionDataStore;
            _applicationUserDataStore = applicationUserDataStore;
        }
        /// <summary>
        /// Получение залогоненого пользователя
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var t = await _applicationCompanionDataStore.GetItemAsync("1");
            var r = await _applicationUserDataStore.GetItemAsync("1");
            return Json(r);
            //return Ok($"Ваш логин: {User.Identity.Name}");
        }
    }
}