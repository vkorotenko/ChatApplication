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
        private DbContext _ctx;
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
        public UserController(DbContext ctx, ILogger<UserController> logger, IHostingEnvironment appEnvironment)
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
            var user = await _ctx.Users.GetUserBuName(User.Identity.Name);
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
            var user = await _ctx.Users.GetUserBuName(User.Identity.Name);
            var msg = new DbMessage
            {
                AuthorId = user.Id,
                Body = message.Body,
                Created = DateTime.Now,
                IsRead = false,
                TopicId = id
            };
            try
            {
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
            var user = _ctx.Users.GetUserBuName(User.Identity.Name);
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
            return Json(files);
        }
    }
}