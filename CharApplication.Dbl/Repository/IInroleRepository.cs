#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  20.04.2019 8:25
#endregion
using ChatApplication.Dbl.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatApplication.Dbl.Repository
{
    public interface IInroleRepository
    {
        /// <summary>
        /// Получение роли пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DbUserInRole> Get(int id);
        /// <summary>
        /// Получение списка пользователей в ролях
        /// </summary>
        /// <returns></returns>
        Task<List<DbUserInRole>> GetAll();
    }
}