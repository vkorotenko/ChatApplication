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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ChatApplication.Dbl.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

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
        private IDbContext _ctx;
        /// <summary>
        /// Логгер
        /// </summary>
        private ILogger _logger;

        /// <summary>
        /// Переменные среды.
        /// </summary>
        private IHostingEnvironment _appEnvironment;
        /// <summary>
        /// Контроллер для отображения пользовательской информации.
        /// </summary>
        /// <param name="ctx">Контекст бд</param>
        /// <param name="logger">Логгер</param>
        /// <param name="appEnvironment">Переменные среды</param>
        public UserController(IDbContext ctx, ILogger<UserController> logger, IHostingEnvironment appEnvironment)
        {
            _ctx = ctx;
            _logger = logger;
            _appEnvironment = appEnvironment;
        }
        /// <summary>
        /// Получение залогоненого пользователя
        /// </summary>
        /// <returns>Возвращает основной обьект пользователя, содержит данные о открытых топиках и количестве непрочтенных сообщений.</returns>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                var user = await _ctx.Users.GetUserBuName(User.Identity.Name);
                var appUser = Mapper.Map<ApplicationUser>(user);
                var topics = await _ctx.Topics.GetByUserId(user.Id);
                var unreadMessage = await _ctx.Users.GetUnreadMessages(user.Id);
                foreach (var topic in topics)
                {
                    if (topic.Unread > 0)
                        topic.HasMessages = true;
                }

                appUser.Topics = topics;
                appUser.NewMessages = (int)unreadMessage;
                return Json(appUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }

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
        /// <summary>
        /// Добавление сообщения в топик. Используется идентификатор залогоненого пользователя.
        /// </summary>
        /// <param name="id">Идентификатор топика</param>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("addtotopic/{id}")]
        public async Task<ActionResult> AddToTopic([FromRoute] long id, [FromBody]AddMessageModel message)
        {
            try
            {
                var user = await _ctx.Users.GetUserBuName(User.Identity.Name);
                var msg = new DbMessage
                {
                    AuthorId = user.Id,
                    Body = message.Body,
                    Created = DateTime.Now,
                    IsRead = false,
                    TopicId = id
                };
                var dbMessage = await _ctx.Messages.Create(msg);
                var model = Mapper.Map<MessageModel>(dbMessage);
                return Json(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        /// <summary>
        /// Загрузка и сохранение файла 
        /// </summary>
        /// <param name="uploads">Коллекция файлов</param>
        /// <param name="topicId">Топик в который грузим</param>
        /// <param name="messageid">Идентификатор сообщения</param>
        /// <returns></returns>
        [HttpPost]
        [Route("addfiles/{topicid}/{messageid}")]
        public async Task<IActionResult> AddFiles(IFormFileCollection uploads, [FromRoute]long topicId, [FromRoute]long messageid)
        {
            var files = new List<UploadFile>();
            try
            {

                var user = await _ctx.Users.GetUserBuName(User.Identity.Name);
                foreach (var uploadedFile in uploads)
                {
                    // Базовый путь
                    var path = Path.Combine(_appEnvironment.WebRootPath, "upload/topic");
                    path = Path.Combine(path, topicId.ToString());
                    try
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        path = Path.Combine(path, uploadedFile.FileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await uploadedFile.CopyToAsync(fileStream);
                        }

                        var dbf = new DbFile
                        {
                            AuthorId = user.Id,
                            Created = DateTime.Now,
                            MessageId = messageid,
                            Name = uploadedFile.FileName
                        };
                        var upfile = await _ctx.Files.Create(dbf);
                        var result = Mapper.Map<UploadFile>(upfile);
                        files.Add(result);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        return BadRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
            return Json(files);
        }
        /// <summary>
        /// Получение общего количества непрочитанных пользователей для пользователя.
        /// </summary>
        /// <param name="userid">Числовой идентификатор пользователя, поле в базе id</param>
        /// <returns>0 по умолчанию или при ошибках, количество непрочтенных во всех топиках в другом</returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("totalunread/{userid}")]
        public async Task<IActionResult> UnreadMessagesForUser([FromRoute]int userid)
        {
            long count = 0;
            try
            {
                count = await _ctx.Users.GetUnreadMessages(userid);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }            
            return Content(count.ToString());
        }
        
        /// <summary>
        /// Устанавливаем флаг прочтения в топике.
        /// </summary>
        /// <param name="topicid"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("clearmessages/{topicid}")]
        public async Task<ActionResult> ClearNewMessagesInTopic([FromRoute] long topicid)
        {
            try
            {
                await _ctx.Messages.MarkMessagesInTopikAsRead(topicid);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        /// <summary>
        /// Получение списка обьявлений, доступных для авторизованного пользователя.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("articles")]
        public async Task<IActionResult> Articles()
        {
            try
            {
                var user = await _ctx.Users.GetUserBuName(User.Identity.Name);
                var articles = await _ctx.Articles.GetAllFromUser(user.Id);
                return Json(articles);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }
    }
}