#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  19.04.2019 23:42
#endregion


using ChatApplication.Dbl.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatApplication.Dbl.Repository
{
    public interface IUserRepository
    {
        Task<DbUser> Create(DbUser user);
        Task Delete(int id);
        Task<DbUser> Get(int id);
        Task <List<DbUser>> GetUsers();
        Task Update(DbUser user);
        /// <summary>
        /// Получение пользователя по имени в системе
        /// </summary>
        /// <param name="username">Имя пользователя в системе</param>
        /// <returns></returns>
        Task<DbUser> GetUserBuName(string username);
    }
}