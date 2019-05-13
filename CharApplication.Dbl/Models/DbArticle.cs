#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  28.04.2019 21:22
#endregion


using System.Collections.Generic;

namespace ChatApplication.Dbl.Models
{
    /// <summary>
    /// Модель обьявлений
    /// </summary>
    public class DbArticle
    {       
        /// <summary>
        /// Идентификатор обьявления
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        ////Название обьявления
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Код, вендора
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Автор
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Производитель
        /// </summary>
        public string Vendor { get; set; }

        public string Photo { get; set; }
        public IEnumerable<ArticleImage> Photos { get; set; }
    }
}
