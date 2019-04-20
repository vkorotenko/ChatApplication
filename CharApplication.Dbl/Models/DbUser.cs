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
        public long Id { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }
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
        public string FullName => $"{LastName} {Name} {MiddleName}";

       
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

    }
}