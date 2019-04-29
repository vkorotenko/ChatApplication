#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  29.04.2019 10:58
#endregion

using System;

namespace ChatApplication.Models
{
    /// <summary>
    /// Файл аттачмент
    /// </summary>
    public class AttachmentModel
    {
        /// <summary>
        /// Имя файла
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// Адрес по которому доступен файл
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Является ли файл картинкой
        /// </summary>
        public bool IsImage { get; set; }
    }
}
