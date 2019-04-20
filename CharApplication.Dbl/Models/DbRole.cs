#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  20.04.2019 8:13
#endregion

namespace ChatApplication.Dbl.Models
{
    /// <summary>
    /// Роль пользователя в системе
    /// </summary>
    public class DbRole
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Имя роли
        /// </summary>
        public string Name { get; set; }
    }
}
