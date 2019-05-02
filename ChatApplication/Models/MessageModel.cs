#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  25.04.2019 13:22
#endregion

using System;
using ChatApplication.Dbl.Models;

namespace ChatApplication.Models
{
    /// <summary>
    /// Модель для отображения сообщения.
    /// </summary>
    public class MessageModel
    {
        /// <summary>
        /// Дефолтный конструктор для модели.
        /// </summary>
        public MessageModel()
        {
            Attachment = new AttachmentModel();
        }
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
        /// <summary>
        /// Картинка на профиль.
        /// </summary>
        public string AvatarUrl => $"/api/v1/user/avatar/{AuthorId}";        
        /// <summary>
        /// Файл прикрепленный к сообщению
        /// </summary>
        public AttachmentModel Attachment { get; set; }
    }
}
