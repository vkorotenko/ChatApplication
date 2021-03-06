﻿#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  20.04.2019 8:30
#endregion
using ChatApplication.Dbl.Models;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApplication.Dbl.Repository
{
    /// <summary>
    /// Репозиторий ролей
    /// </summary>
    public class RoleRepository : IRoleRepository
    {
        private IDbConnection _db;
        /// <summary>
        /// Конструктор репозитория ролей
        /// </summary>
        /// <param name="dbConnection"></param>
        public RoleRepository(IDbConnection dbConnection)
        {
            _db = dbConnection;
        }
        /// <summary>
        /// Получение роли по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DbRole> Get(int id)
        {
            var sql = @"SELECT item_name as name FROM admin_zap.rbac_auth_assignment WHERE user_id = @id";
            return (await _db.QueryAsync<DbRole>(sql, new { id })).FirstOrDefault();
        }

        /// <summary>
        /// Получение списка ролей
        /// </summary>
        /// <returns></returns>
        public async Task<List<DbRole>> GetRoles()
        {
            return (await _db.QueryAsync<DbRole>("SELECT distinct(item_name) as name FROM admin_zap.rbac_auth_assignment")).ToList();
        }

        /// <summary>
        /// Получение списка ролей для пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <returns>Список ролей где есть пользователь</returns>
        public async Task<List<DbRole>> GetRolesForUser(int id)
        {            
            var sql = @"SELECT item_name as name FROM admin_zap.rbac_auth_assignment WHERE user_id = @id";
            return (await _db.QueryAsync<DbRole>(sql, new { id })).ToList();
        }
    }
}