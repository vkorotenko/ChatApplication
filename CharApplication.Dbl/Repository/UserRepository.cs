#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  19.04.2019 22:06
#endregion

using System;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ChatApplication.Dbl.Models;

namespace ChatApplication.Dbl.Repository
{
    public class UserRepository : IUserRepository
    {
        readonly IDbConnection _dbConn = null;
        public UserRepository(IDbConnection dbConnection)
        {
            _dbConn = dbConnection;
        }
        public async Task<List<DbUser>> GetUsers()
        {
            var sql = @"SELECT u.id, u.username, u.email, u.generate_password as password, up.firstname, up.middlename, up.lastname,   concat( up.avatar_base_url , up.avatar_path) as url  
                        FROM admin_zap.user as u 
                        INNER JOIN admin_zap.user_profile as up
                        ON u.id = up.user_id
                        ";
            return (await _dbConn.QueryAsync<DbUser>(sql)).ToList();
        }

        public async Task<DbUser> Get(int id)
        {
            var sql = @"SELECT u.id, u.username, u.email, u.generate_password as password, up.firstname, up.middlename, up.lastname,   concat( up.avatar_base_url , up.avatar_path) as url  
                        FROM admin_zap.user as u 
                        INNER JOIN admin_zap.user_profile as up
                        ON u.id = up.user_id
                        WHERE u.id = @id";
            return (await _dbConn.QueryAsync<DbUser>(sql, new { id })).FirstOrDefault();

        }

        public async Task<DbUser> Create(DbUser user)
        {
            // Доступ только на чтение
            throw new NotImplementedException();
            
        }


        public async Task Update(DbUser user)
        {
            // Доступ только на чтение
            throw new NotImplementedException();
           

        }

        /// <summary>
        /// Получение пользователя по имени в системе
        /// </summary>
        /// <param name="username">Имя пользователя в системе</param>
        /// <returns></returns>
        public async Task<DbUser> GetUserBuName(string username)
        {
            var sql = @"SELECT u.id, u.username, u.email, u.generate_password as password, up.firstname, up.middlename, up.lastname,   concat( up.avatar_base_url , up.avatar_path) as url  
                        FROM admin_zap.user as u 
                        INNER JOIN admin_zap.user_profile as up
                        ON u.id = up.user_id
                        WHERE u.username = @username";
            var users = await _dbConn.QueryAsync<DbUser>(sql, new { username });
            var user = users.FirstOrDefault();
            return user;
        }

        /// <summary>
        /// Получение количества непрочтенных писем
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<long> GetUnreadMessages(int userId)
        {
            var sql = @"SELECT count(isread) as count
                          FROM dbmessages
                         WHERE topicid IN (SELECT id
                                           FROM dbtopics
                                          WHERE authorid=@userId) AND NOT isread";


            var results = await _dbConn.QueryAsync<long>(sql, new { userId });
            var result = results.FirstOrDefault();
            return result;
        }

        /// <summary>
        /// Получение общего количества писем для пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<long> GetTotalMessages(int userId)
        {
            var sql = @"SELECT count(isread) as count
                          FROM dbmessages
                         WHERE topicid IN (SELECT id
                                           FROM dbtopics
                                          WHERE authorid=@userId)";


            var results = await _dbConn.QueryAsync<long>(sql, new { userId });
            var result = results.FirstOrDefault();
            return result;
        }

        public async Task Delete(int id)
        {
            // только чтение
            throw new NotImplementedException();            
        }
    }
}
