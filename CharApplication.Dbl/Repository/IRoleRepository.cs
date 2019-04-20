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
    /// <summary>
    /// Интерфейс репозитория ролей
    /// </summary>
    public interface IRoleRepository
    {
        /// <summary>
        /// Получение роли по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DbRole> Get(int id);
        /// <summary>
        /// Получение списка ролей
        /// </summary>
        /// <returns></returns>
        Task<List<DbRole>> GetRoles();
    }
}