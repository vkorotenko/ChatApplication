#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  13.05.2019 6:11
#endregion

using ChatApplication.Dbl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ChatApplication.Controllers
{
    /// <summary>
    /// Контроллер для отображения обьявлений
    /// </summary>
    [Route("api/v1/article")]
    [ApiController]
    public class ArticleController : Controller
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
        /// Конструктор контроллера обьявлений
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="logger"></param>
        /// <param name="appEnvironment"></param>
        /// <param name="config"></param>
        public ArticleController(IDbContext ctx, ILogger<UserController> logger, IHostingEnvironment appEnvironment,
            IConfiguration config)
        {
            _ctx = ctx;
            _logger = logger;
            _appEnvironment = appEnvironment;
            _config = config;
        }

        /// <summary>
        /// Получение изображения для обьявления.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("{id}/img")]
        public async Task<ActionResult> Img([FromRoute] int id)
        {
            var failUrl = "/img/no-image.png";
            try
            {

                var article = await _ctx.Articles.Get(id);
                var image = article.Photos.FirstOrDefault();

                if (image ==null)
                    return Redirect(failUrl);
                else
                {
                   return Redirect(GenerateThnumnail(image.Photo));
                }                                
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
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
    }
}