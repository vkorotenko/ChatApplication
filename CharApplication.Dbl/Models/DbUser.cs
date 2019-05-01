using System;

namespace ChatApplication.Dbl.Models
{
    /// <summary>
    /// Пользователь в базе данных
    /// </summary>   
    public class DbUser
    {
        /// <summary>
        /// Телефон пользователя, уникальный в системе.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Имя пользователя в системе
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName { get; set; }
        /// <summary>
        /// Вычисляемое поле для полного имени
        /// </summary>
        public string FullName => $"{LastName} {FirstName} {MiddleName}";

       
        /// <summary>
        /// Email пользователя
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Время последней активности
        /// </summary>
        public DateTime LastActive { get; set; }
        /// <summary>
        /// url аватара
        /// </summary>
        public string Url { get; set; }
    }
}