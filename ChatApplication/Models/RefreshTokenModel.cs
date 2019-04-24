#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  24.04.2019 11:05
#endregion

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ChatApplication.Models
{
    /// <summary>
    /// Модель обновления токена
    /// </summary>
    public class RefreshTokenModel
    {
        /// <summary>
        /// Токен для обновления
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
