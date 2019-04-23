#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  20.04.2019 11:51
#endregion


using System;

namespace ChatApplication.Dbl.Models
{
    /// <summary>
    /// Сообщения в системе
    /// </summary>
    public class DbMessage
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Тело сообщения
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// Автор сообщения
        /// </summary>
        public int AuthorId { get; set; }
        /// <summary>
        /// Флаг прочтения
        /// </summary>
        public bool IsRead { get; set; }
        /// <summary>
        /// Тред в котором создано сообщение
        /// </summary>
        public long TopicId { get; set; }
    }
}
