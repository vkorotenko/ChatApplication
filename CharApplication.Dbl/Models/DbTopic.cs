#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  20.04.2019 9:29
#endregion

using System;
using System.Diagnostics.Tracing;

namespace ChatApplication.Dbl.Models
{
    /// <summary>
    /// Класс для доступа к темам
    /// </summary>
    public class DbTopic
    {
        /// <summary>
        /// Дефолтный конструктор
        /// </summary>
        public DbTopic()
        {
            HasMessages = false;
            Unread = 0;
        }
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Заголовок, берется из заголовка сообщения
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        public long AnnouncementId { get; set; }
        /// <summary>
        /// Производитель берется из начального сообщения
        /// </summary>
        public string Vendor { get; set; }
        /// <summary>
        /// Код производителя, берем из начального сообщения
        /// </summary>
        public string VendorCode { get; set; }
        /// <summary>
        /// Идентификатор человека создавшего запрос.
        /// </summary>
        public int AuthorId { get; set; }
        /// <summary>
        /// Дата создания, автоматически добавляется во время вставки
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// Есть ли сообщения в списке.
        /// </summary>
        public bool HasMessages { get; set; }
        /// <summary>
        /// Количество непрочтенных сообщений
        /// </summary>
        public int Unread { get; set; }
        /// <summary>
        /// Аватар автора топика.
        /// </summary>
        public string AvatarUrl => $"/api/v1/user/avatar/{AuthorId}";
        /// <summary>
        /// Время последнего обновления топика.
        /// </summary>
        public DateTime Updated { get; set; }
    }
}
