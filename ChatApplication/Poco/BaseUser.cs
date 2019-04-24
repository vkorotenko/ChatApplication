#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  18.04.2019 8:50
#endregion

using System;
using Newtonsoft.Json;

namespace ChatApplication.Poco
{
    /// <summary>
    /// Базовый класс для всех пользователей
    /// </summary>
    public class BaseUser
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
        /// <summary>
        /// Имя пользователя
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// Описание или девиз.
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }
        
        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName { get; set; }
        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }
        /// <summary>
        /// Пароль пользователя
        /// </summary>        
        [JsonIgnore]
        public string Password { get; set; }
    }
}