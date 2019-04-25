#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  25.04.2019 23:03
#endregion

using Newtonsoft.Json;

namespace ChatApplication.Models
{
    /// <summary>
    /// Модель для добавления сообщения
    /// </summary>
    public class AddMessageModel
    {
        /// <summary>
        /// Тело сообщения
        /// </summary>
        [JsonProperty("body")]
        public string Body { get; set; }
    }
}
