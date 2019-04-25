#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  17.04.2019 23:09
#endregion

using AutoMapper;
using ChatApplication.Dbl;
using ChatApplication.Models;
using ChatApplication.Poco;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var user = users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            var appUser = Mapper.Map<ApplicationUser>(user);
            var topics = await _ctx.Topics.GetByUserId(user.Id);

            var i = 0;
            foreach (var topic in topics)
            {
                if (i == 2)
                {
                    topic.HasMessages = true;
                    topic.Unread = 5;
                }
                i++;
            }
            appUser.Topics = topics;
            //TODO: удалить фейк
            appUser.NewMessages = 122;
            return Json(appUser);
        }

        /// <summary>
        /// Получение списка сообщений для топика. 
        /// </summary>
        /// <param name="id">Идентификатор топика</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("messages/{id}")]
        public async Task<ActionResult> Messages([FromRoute] long id)
        {
            _logger.LogInformation($"Retrive data for topic id: {id}");
            try
            {
                var messages = await _ctx.Messages.GetMessagesForTopic(id);
                var rt = Mapper.Map<IEnumerable<MessageModel>>(messages);
                return Json(rt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        /// <summary>
        /// Получаем аватар по идентификатору пользователя.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("avatar/{id}")]
        public ActionResult Avatar([FromRoute] long id)
        {
            //TODO: добавить логику для выбора картинки по идентификатору пользователя.
            var url = "/upload/faceses/1.png";
            return RedirectPermanent(url);
        }
    }
}