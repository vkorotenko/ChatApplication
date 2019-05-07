#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  06.05.2019 17:52
#endregion

using System;
using Newtonsoft.Json;

namespace ChatApplication.Models
{
    /// <summary>
    /// Последние новости для отображения сверху
    /// </summary>
    public class LatestMessageModel
    {
        /// <summary>
        /// Полное имя автора
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Заголовок сообщения
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// Прямой Url для открытия новости.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Автор
        /// </summary>
        [JsonIgnore]
        public int AuthorId { get; set; }

        /// <summary>
        /// Тред в котором создано сообщение
        /// </summary>
        [JsonIgnore]
        public long TopicId { get; set; }

        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        [JsonIgnore]
        public long Id { get; set; }
    }
}
