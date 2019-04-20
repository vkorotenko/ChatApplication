#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  20.04.2019 16:26
#endregion

using System;

namespace ChatApplication.Dbl.Models
{
    /// <summary>
    /// Файл в чате
    /// </summary>
    public class DbFile
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Имя файла
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Автор
        /// </summary>
        public long AuthorId { get; set; }
        /// <summary>
        /// Создано
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// Сообщение к которому прикреплено
        /// </summary>
        public long MessageId { get; set; }
    }
}
