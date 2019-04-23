#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  14.04.2019 7:44
#endregion
using Newtonsoft.Json;

namespace ChatApplication.Models
{
    /// <summary>
    /// Модель для входа в приложение
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Имя пользователя, для обычного используется номер телефона пользователя.
        /// </summary>
        [JsonProperty("username")]
        public string UserName { get; set; }
        /// <summary>
        /// Пароль для входа.
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
