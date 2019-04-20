#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  20.04.2019 8:40
#endregion
namespace ChatApplication.Dbl.Models
{
    public class DbUserInRole
    {
        public long Id { get; set; }
        public int RoleId { get; set; }
        public long UserId { get; set; }
    }
}
