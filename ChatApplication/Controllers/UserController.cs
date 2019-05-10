#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  17.04.2019 23:09
#endregion

using AutoMapper;
using ChatApplication.Dbl;
using ChatApplication.Dbl.Models;
using ChatApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ChatApplication.Code;

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
        public async Task<ActionResult> Messages([FromRoute] long id)
        {
            _logger.LogInformation($"Retrive data for topic id: {id}");
            try
            {
                var messages = await _ctx.Messages.GetMessagesForTopic(id);
                var rt = Mapper.Map<IEnumerable<MessageModel>>(messages);

                foreach (var message in rt)
                {
                    var dbattachment = await _ctx.Files.GetForMessage(message.Id);
                    var attachment = Mapper.Map<AttachmentModel>(dbattachment);
                    if (attachment != null)
                    {
                        attachment.Url = $"/upload/topic/{id}/{attachment.Name}";
                        attachment.IsImage = IsImage(attachment.Name);
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
            var def = "/img/ava.gif";
            try
            {
                var user = await _ctx.Users.Get(id);
                var url = user.Url;
                if (string.IsNullOrWhiteSpace(url))
                    url = def;
                return url;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError($"Failed retrive awatar image user id: {id}");
                return def;
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
                message.Body = message.Body.Replace("\n", "<br/>");
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
            var allowedExtension = new string[]{
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
                            var errmsg =  "Неподдерживаемый тип файла, разрешены только следующие расширения: " + string.Join(" ,", allowedExtension);
                            await _ctx.Messages.Delete(messageid);
                            return StatusCode(415,errmsg);                            
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
                var roles = await _ctx.Roles.GetRolesForUser(userid);
                if (await IsAdminOrManager(userid))
                {
                    count = await _ctx.Messages.GetUnreadMessagesForAdmin(userid);
                }
                else
                {
                    count = await _ctx.Messages.GetUnreadMessages(userid);
                }
                
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            return Content(count.ToString());
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
        /// Получение списка обьявлений, доступных для авторизованного пользователя.
        /// Служебный метод для получения списка обьявлений.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("articles")]
        public async Task<IActionResult> Articles()
        {
            try
            {
                var user = await _ctx.Users.GetUserByName(User.Identity.Name);
                var articles = await _ctx.Articles.GetAllFromUser(user.Id);
                return Json(articles);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
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
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
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
                query = query.ToLower();
                var user = await _ctx.Users.GetUserByName(User.Identity.Name);
                var appUser = Mapper.Map<ApplicationUser>(user);
                List<DbTopic> matchTopics;
                if (await IsAdminOrManager(user.Id))
                {
                    matchTopics = await _ctx.Topics.GetByAdminId(user.Id);
                }
                else
                {
                    matchTopics = await _ctx.Topics.GetByUserId(user.Id);
                }

                // поиск по пустой строке, все результаты
                if (!string.IsNullOrWhiteSpace(query)){                

                    matchTopics = matchTopics.Where(x => x.Title.ToLower().Contains(query)
                                      || x.Vendor.ToLower().Contains(query)
                                      || x.VendorCode.ToLower().Contains(query)).ToList();
                }

                var authId = -1;
                var url = string.Empty;
                foreach (var topic in matchTopics)
                {
                    topic.FullName = user.FullName;
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

                appUser.Topics = matchTopics;
                var unreadMessage = await _ctx.Messages.GetUnreadMessages(user.Id);
                appUser.NewMessages = (int)unreadMessage;
                return Json(appUser);
            }
            catch (Exception e)
            {
                _logger.LogError($"Search error. {e.Message}");
                return BadRequest();
            }
        }
        /// <summary>
        /// Получаем последние 4 непрочтенных сообщения для пользователя.
        /// Либо возвращаем пустой список если их нет.
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
    }
}