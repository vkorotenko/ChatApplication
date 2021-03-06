﻿#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  17.04.2019 23:09
#endregion

using AutoMapper;
using ChatApplication.Code;
using ChatApplication.Dbl;
using ChatApplication.Dbl.Models;
using ChatApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ChatApplication.Bl;

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
        /// Конфигурация
        /// </summary>
        private IConfiguration _config;
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
        /// <param name="config">Конфигурация системы</param>
        public UserController(IDbContext ctx, ILogger<UserController> logger, IHostingEnvironment appEnvironment, IConfiguration config)
        {
            _ctx = ctx;
            _logger = logger;
            _appEnvironment = appEnvironment;
            _config = config;
        }
        /// <summary>
        /// Получение залогоненого пользователя
        /// </summary>
        /// <returns>Возвращает основной обьект пользователя, содержит данные о открытых топиках и количестве непрочтенных сообщений.</returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await Search(string.Empty);
        }

        /// <summary>
        /// Получение списка сообщений для топика. 
        /// </summary>
        /// <param name="id">Идентификатор топика</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("messages/{id}")]
        public async Task<ActionResult> Messages([FromRoute] int id)
        {
            _logger.LogInformation($"Retrive data for topic id: {id}");
            try
            {
                await CreateTopicIfNotExist(id);
                var user = await _ctx.Users.GetUserByName(User.Identity.Name);
                var messages = await _ctx.Messages.GetMessagesForTopic(id);
                var rt = Mapper.Map<IEnumerable<MessageModel>>(messages);
                var days = new List<string>();
                var users = new Dictionary<int, DbUser>();
                var basePath = _config.GetValue<string>("Upload:Path");
                foreach (var message in rt)
                {
                    Debug.WriteLine(message.Id);
                    if (message.AuthorId == user.Id)
                        message.IsAuthor = true;
                    var day = $"{message.Created.Year}_{message.Created.Month}_{message.Created.Day}";
                    if (!days.Contains(day))
                    {
                        message.NewDay = true;
                        days.Add(day);
                    }

                    DbUser msguser;
                    if (!users.TryGetValue(message.AuthorId, out msguser))
                    {
                        msguser = await _ctx.Users.Get(message.AuthorId);
                        if (msguser != null)
                            users.Add(msguser.Id, msguser);
                    }

                    if (msguser != null)
                    {
                        message.FullName = msguser.FullName;
                        message.Name = msguser.FirstName;
                    }

                    var dbattachment = await _ctx.Files.GetForMessage(message.Id);
                    var attachment = Mapper.Map<AttachmentModel>(dbattachment);

                    if (attachment != null)
                    {
                        attachment.Url = $"/upload/topic/{id}/{attachment.Name}";
                        attachment.IsImage = IsImage(attachment.Name);
                        var fp = Path.Combine(basePath, $"topic/{id}/{attachment.Name}");
                        if (System.IO.File.Exists(fp))
                            attachment.Size = new FileInfo(fp).Length;

                        message.Attachment = attachment;
                    }
                    else
                    {
                        message.Attachment = new AttachmentModel();
                    }
                }
                return Json(rt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        /// <summary>
        /// Определение является ли файл картинкой
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static bool IsImage(string name)
        {
            var nameLower = name.ToLower();

            if (nameLower.EndsWith(".jpg") ||
                nameLower.EndsWith(".jpeg") ||
                nameLower.EndsWith(".png") ||
                nameLower.EndsWith(".gif") ||
                nameLower.EndsWith(".bmp") ||
                nameLower.EndsWith(".webm")
            )
                return true;
            return false;
        }

        /// <summary>
        /// Получаем аватар по идентификатору пользователя.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("avatar/{id}")]
        public async Task<IActionResult> Avatar([FromRoute] int id)
        {
            var url = await GetAvatarImage(id);
            return Redirect(url);
        }

        private async Task<string> GetAvatarImage(int id)
        {
            var avatarImage = "/img/ava.gif";
            try
            {
                var user = await _ctx.Users.Get(id);
                var url = user.Url;
                if (string.IsNullOrWhiteSpace(url))
                    url = avatarImage;
                else
                {
                    url = GenerateThnumnail(url);
                }
                return url;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError($"Failed retrive awatar image user id: {id}");
                return avatarImage;
            }
        }
        /// <summary>
        /// Создаем превьюшку аватарки при необходимости.
        /// </summary>
        /// <param name="url"></param>
        private string GenerateThnumnail(string url)
        {
            try
            {
                var basePath = _config.GetValue<string>("Upload:AppDir");
                var nurl = url.Remove(0, 1);
                var fullPath = Path.Combine(basePath, nurl);

                var fi = new FileInfo(fullPath);

                var ext = fi.Extension;
                var name = fi.Name.Replace(ext, "");
                var th = $"{name}_50{ext}";
                url = url.Replace($"{name}{ext}", th);
                nurl = url.Remove(0, 1);
                var fullThPath = Path.Combine(basePath, nurl);


                if (System.IO.File.Exists(fullThPath))
                {
                    return url;
                }
                else
                {
                    using (Image<Rgba32> image = Image.Load(fullPath))
                    {
                        image.Mutate(x => x.Resize(50, 50));
                        image.Save(fullThPath);
                    }
                    return url;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GenerateThnumnail");
                return url;
            }

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
                var user = await _ctx.Users.GetUserByName(User.Identity.Name);

                message.Body = Regex.Replace(message.Body, "<.*?>", string.Empty);
                message.Body = message.Body.Replace("\r\n", "\n");
                var msgstring = message.Body.Split(new[] { "\n" }, StringSplitOptions.None);
                message.Body = string.Join("<br/>", msgstring);
                var msg = new DbMessage
                {
                    AuthorId = user.Id,
                    Body = message.Body,
                    Created = DateTime.Now,
                    IsRead = false,
                    TopicId = id
                };
                var dbMessage = await _ctx.Messages.Create(msg);
                await _ctx.Topics.UpdateTs(id);
                var model = Mapper.Map<MessageModel>(dbMessage);
                model.IsAuthor = true;

                MessagePolling.Publish(user.Id, new LpMessage
                {
                    Name = user.FirstName,
                    id = user.Id,
                    Topic = id,
                    Unread = 1
                });
                return Json(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        /// <summary>
        /// Загрузка и сохранение файла в топик. 
        /// </summary>
        /// <param name="uploads">Коллекция файлов</param>
        /// <param name="topicId">Топик в который грузим</param>
        /// <param name="messageid">Идентификатор сообщения</param>
        /// <returns></returns>
        [HttpPost]
        [Route("addfiles/{topicid}/{messageid}")]
        public async Task<IActionResult> AddFiles(IFormFileCollection uploads, [FromRoute]long topicId, [FromRoute]long messageid)
        {
            var allowedExtension = AllowedExtensionFromConfig();
            var files = new List<UploadFile>();
            var gattachment = new AttachmentModel();
            try
            {

                var basePath = _config.GetValue<string>("Upload:Path");
                var user = await _ctx.Users.GetUserByName(User.Identity.Name);
                foreach (var uploadedFile in uploads)
                {
                    // Базовый путь
                    var path = Path.Combine(basePath, "topic");
                    path = Path.Combine(path, topicId.ToString());
                    try
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        var fname = DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + $"_{uploadedFile.FileName}";
                        fname = fname.ToLower();
                        var fileExt = System.IO.Path.GetExtension(fname);
                        if (!allowedExtension.Contains(fileExt))
                        {
                            var errmsg = "Неподдерживаемый тип файла, разрешены только следующие расширения: " + string.Join(" ,", allowedExtension);
                            await _ctx.Messages.Delete(messageid);
                            return StatusCode(415, errmsg);
                        }
                        path = Path.Combine(path, fname);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await uploadedFile.CopyToAsync(fileStream);
                        }

                        var dbf = new DbFile
                        {
                            AuthorId = user.Id,
                            Created = DateTime.Now,
                            MessageId = messageid,
                            Name = fname
                        };
                        var upfile = await _ctx.Files.Create(dbf);
                        var result = Mapper.Map<UploadFile>(upfile);

                        var dbattachment = await _ctx.Files.GetForMessage(messageid);
                        var attachment = Mapper.Map<AttachmentModel>(dbattachment);
                        if (attachment != null)
                        {
                            attachment.Url = $"/upload/topic/{topicId}/{attachment.Name}";
                            attachment.IsImage = IsImage(attachment.Name);
                            gattachment = attachment;
                        }

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
            return Json(gattachment);
        }

        private string[] AllowedExtensionFromConfig()
        {
            var allowedExtension = new string[]
            {
                ".png",
                ".jpeg",
                ".jpg",
                ".gif",
                ".txt",
                ".bmp",
                ".zip",
                ".rar",
                ".doc",
                ".docx",
                ".xlsx",
                ".txt"
            };
            try
            {
                var exfiles = _config.GetValue<string>("AllowedExtension");
                var exarr = exfiles.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                allowedExtension = exarr;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return allowedExtension;
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
                count = await GetUnreadMessages(userid);
                var lp = new MessagePolling(userid);
                Debug.WriteLine("Send unread: totalunread");
                var message = await lp.WaitAsync() ?? new LpMessage { Unread = (int)count };
                message.Unread = (int)count;
                Debug.WriteLine("Send unread: totalunread");
                return new JsonResult(message);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            return Json(new LpMessage { Unread = (int)count });
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("startunread/{id}")]
        public async Task<IActionResult> StartUnread([FromRouteAttribute] int id)
        {
            long count = 0;
            try
            {
                count = await GetUnreadMessages(id);
                return Content(count.ToString());
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            return Json(new LpMessage { Unread = (int)count });
        }
        private async Task<long> GetUnreadMessages(int userid)
        {
            long count = 0;
            var roles = await _ctx.Roles.GetRolesForUser(userid);
            if (await IsAdminOrManager(userid))
            {
                count = await _ctx.Messages.GetUnreadMessagesForAdmin(userid);
            }
            else
            {
                count = await _ctx.Messages.GetUnreadMessages(userid);
            }

            return count;
        }


        private async Task<bool> IsAdminOrManager(int id)
        {
            var roles = await _ctx.Roles.GetRolesForUser(id);
            if (roles.FirstOrDefault(x => x.Name == UserRoles.Administrator || x.Name == UserRoles.Manager) !=
                null) return true;
            else return false;
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
                var user = await _ctx.Users.GetUserByName(User.Identity.Name);
                await _ctx.Messages.MarkMessagesInTopikAsRead(topicid, user.Id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        /// <summary>
        /// Создание топика на основе обьявления
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("createtopic/{id}")]
        public async Task<IActionResult> CreateTopic([FromRoute]int id)
        {
            _logger.LogInformation($"Create topic from article id: {id}");
            try
            {
                await CreateTopicIfNotExist(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        private async Task CreateTopicIfNotExist(int id)
        {
            var topic = await _ctx.Topics.Get(id);
            if (topic == null)
            {
                var article = await _ctx.Articles.Get((int)id);
                if (article != null)
                {
                    var newTopic = new DbTopic
                    {
                        Id = article.Id,
                        AnnouncementId = article.Id,
                        AuthorId = article.UserId,
                        Created = DateTime.Now,
                        Updated = DateTime.Now,
                        Title = article.Title,
                        Vendor = article.Vendor,
                        VendorCode = article.Code
                    };
                    await _ctx.Topics.Create(newTopic);
                }
            }
        }

        /// <summary>
        /// Поиск топиков соответствующих шаблону. Поиск происходит слудующим полям:
        /// Title - заголовок топика
        /// Vendor - производитель
        /// Vendor code -  код производителя
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("search/{query}")]
        [Route("search")]
        public async Task<IActionResult> Search(string query = "")
        {
            try
            {
                var user = await _ctx.Users.GetUserByName(User.Identity.Name);
                var appUser = Mapper.Map<ApplicationUser>(user);
                List<DbTopic> matchTopics;

                if (await IsAdminOrManager(user.Id))
                    matchTopics = await _ctx.Topics.GetByAdminId(user.Id);
                else
                    matchTopics = await _ctx.Topics.GetByUserId(user.Id);

                FillNames(matchTopics, user, appUser.Name);
                var searchProcessor = new SearchProcessor(matchTopics);
                var resultTopics = searchProcessor.Contains(query);
                await FillResultTopics(matchTopics, user);
                appUser.Topics = resultTopics;
                var unreadMessage = await _ctx.Messages.GetUnreadMessages(user.Id);
                appUser.NewMessages = unreadMessage;
                return Json(appUser);
            }
            catch (Exception e)
            {
                _logger.LogError($"Search error. {e.Message}");
                return BadRequest();
            }
        }
        /// <summary>
        /// Заполняем структуру именами
        /// </summary>
        /// <param name="resultTopics"></param>
        /// <param name="user"></param>
        /// <param name="appUserName"></param>
        /// <returns></returns>
        private void FillNames(IEnumerable<DbTopic> resultTopics, DbUser user, string appUserName)
        {            
            foreach (var topic in resultTopics)
            {
                topic.FullName = user.FullName;
                topic.Name = appUserName;         
            }
        }
        private async Task FillResultTopics(IEnumerable<DbTopic> resultTopics, DbUser user)
        {
            var authId = -1;
            var url = string.Empty;
            foreach (var topic in resultTopics)
            {                
                topic.Price = await _ctx.Articles.GetPrice(topic.AnnouncementId);
                await FillTopicLastMessage(topic, user.Id);
                if (string.IsNullOrWhiteSpace(url))
                {
                    url = await GetAvatarImage(topic.AuthorId);
                    authId = topic.AuthorId;
                }

                if (authId != topic.AuthorId)
                {
                    url = await GetAvatarImage(topic.AuthorId);
                    authId = topic.AuthorId;
                }


                if (topic.Unread > 0)
                    topic.HasMessages = true;
            }
        }

        /// <summary>
        /// Заполняем поля последнего сообщения в топике, признак прочтения и флаг автор или нет
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<DbTopic> FillTopicLastMessage(DbTopic topic, int userId)
        {
            var msg = await _ctx.Messages.GetLastMessageForTopic(topic.Id);
            if (msg != null)
            {
                var msgUser = await GetCachedUser(msg.AuthorId);
                topic.LmAuthorId = msg.AuthorId;
                topic.LmIsCurrent = msg.AuthorId == userId;
                topic.LastMessage = msg.Body;
                topic.LmIsReaded = msg.IsRead;
                topic.LmCreated = msg.Created;
                if (msgUser != null)
                {
                    topic.LmName = msgUser.FirstName;
                }
            }

            return topic;
        }


        private static object _locker = new object();
        private Dictionary<int, DbUser> _cashedUser = new Dictionary<int, DbUser>();

        /// <summary>
        /// Получение закешированного пользователя.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<DbUser> GetCachedUser(int id)
        {
            lock (_locker)
            {
                if (_cashedUser.ContainsKey(id))
                {
                    return _cashedUser[id];
                }
            }
            var u = await _ctx.Users.Get(id);
            lock (_locker)
            {
                _cashedUser.TryAdd(id, u);
            }
            return u;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("getlatestmessages")]
        public async Task<IActionResult> GetLatestMessages()
        {

            try
            {
                var user = await _ctx.Users.GetUserByName(User.Identity.Name);

                List<DbMessage> msg;
                if (await IsAdminOrManager(user.Id))
                {
                    msg = await _ctx.Messages.GetNewMessagesForAdmin(user.Id);
                }
                else
                {
                    msg = await _ctx.Messages.GetNewMessagesForUser(user.Id);
                }

                var models = Mapper.Map<IEnumerable<LatestMessageModel>>(msg);
                models = models
                    .OrderByDescending(x => x.Created)
                    .Take(4).ToList();

                foreach (var model in models)
                {
                    var u = await _ctx.Users.Get(model.AuthorId);
                    model.FullName = u.FullName;
                }

                return Json(models);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Json(new LatestMessageModel[0]);
            }
        }

        /// <summary>
        /// Вызывается при пользовательском вводе на клиенте. Дергает цикл с сообщениями
        /// </summary>
        /// <param name="topicid"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("typing/{topicid}")]
        public async Task<ActionResult> Typing(int topicid)
        {
            try
            {
                var user = await _ctx.Users.GetUserByName(User.Identity.Name);
                MessagePolling.Publish(user.Id, new LpMessage
                {
                    Topic = topicid,
                    Name = user.FirstName,
                    id = user.Id
                });
                return Json(null);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Json(null);
            }
        }
    }
}