#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  20.04.2019 8:52
#endregion
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ChatApplication.Dbl.Models;
using Dapper;

namespace ChatApplication.Dbl.Repository
{
    public class InroleRepository : IInroleRepository
    {
        private IDbConnection _db;
        /// <summary>
        /// Конструктор репозитория ролей
        /// </summary>
        /// <param name="dbConnection"></param>
        public InroleRepository(IDbConnection dbConnection)
        {
            _db = dbConnection;
        }
        /// <summary>
        /// Получение роли пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DbUserInRole> Get(int id)
        {
            return (await _db.QueryAsync<DbUserInRole>("SELECT * FROM dbuserinroles WHERE id = @id", new { id })).FirstOrDefault();
        }

        /// <summary>
        /// Получение списка пользователей в ролях
        /// </summary>
        /// <returns></returns>
        public async Task<List<DbUserInRole>> GetAll()
        {
            return (await _db.QueryAsync<DbUserInRole>("SELECT * FROM dbuserinroles " )).ToList();
        }
    }
}