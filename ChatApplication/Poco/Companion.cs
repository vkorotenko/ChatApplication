#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  18.04.2019 8:49
#endregion

using Newtonsoft.Json;

namespace ChatApplication.Poco
{
    /// <inheritdoc />
    /// <summary>
    /// Класс для компаньона
    /// </summary>
    public class Companion : BaseUser
    {
        /// <summary>
        /// Общее количество новых сообщений.
        /// </summary>
        [JsonProperty("total")]
        public int TotalMessages { get; set; }
        /// <summary>
        /// количество новых
        /// </summary>
        [JsonProperty("newmsg")]
        public int NewMessages { get; set; }
    }
}