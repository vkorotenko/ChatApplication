#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  14.04.2019 7:30
#endregion
using System;

namespace ChatApplication.Models
{
    /// <summary>
    /// Сообщение пользователя
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        public Guid Id{ get; set; }
        /// <summary>
        /// Тело сообщения
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// Является ли сообщение новым
        /// </summary>
        public bool IsNew { get; set; }
    }
}