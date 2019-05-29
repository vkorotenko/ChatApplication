#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  23.05.2019 11:59
#endregion


namespace ChatApplication.Models
{
    public class LpMessage
    {
        /// <summary>
        /// Количество непрочтенных сообщений
        /// </summary>
        public int Unread { get; set; }
        
        /// <summary>
        /// Имя печатающего
        /// </summary>
        public string Name { get; set; }

        public int id { get; set; }
        /// <summary>
        /// Идентификатор топика
        /// </summary>
        public long Topic { get; set; }
    }
}
