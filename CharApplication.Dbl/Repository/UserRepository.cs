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
            var sql = @"SELECT id,username,email, generate_password as password FROM admin_zap.user";
            return (await _dbConn.QueryAsync<DbUser>(sql)).ToList();
        }

        public async Task<DbUser> Get(int id)
        {
            var sql = @"SELECT id,username,email, generate_password as password FROM admin_zap.user WHERE id = @id";
            return (await _dbConn.QueryAsync<DbUser>(sql, new { id })).FirstOrDefault();

        }

        public async Task<DbUser> Create(DbUser user)
        {            
            // Доступ только на чтение
            throw new NotImplementedException();
            var sqlQuery = @"INSERT INTO dbusers (id, username, name, lastname, middlename, email, password, lastactive) 
                            VALUES(@Id, @UserName, @Name, @LastName, @MiddleName, @Email, @Password, @LastActive); 
                            SELECT LAST_INSERT_ID()";
            int? userId = (await _dbConn.QueryAsync<int>(sqlQuery, user)).FirstOrDefault();
            user.Id = userId.Value;
            return user;
        }


        public async Task Update(DbUser user)
        {
            // Доступ только на чтение
            throw new NotImplementedException();
            const string sqlQuery = @"UPDATE dbusers
                                        SET
                                        `id` = @Id,
                                        username = @UserName,
                                        `name` = @Name,
                                        `lastname` = @LastName,
                                        `middlename` = @MiddleName,
                                        `email` = @Email,
                                        `password` = @Password,
                                        `lastactive` = @LastActive
                                        WHERE `id` = @Id ;";
            await _dbConn.ExecuteAsync(sqlQuery, user);

        }

        /// <summary>
        /// Получение пользователя по имени в системе
        /// </summary>
        /// <param name="username">Имя пользователя в системе</param>
        /// <returns></returns>
        public async Task<DbUser> GetUserBuName(string username)
        {
            var users = await _dbConn.QueryAsync<DbUser>("SELECT * FROM admin_zap.user WHERE username = @username",
                new { username });
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
            const string sqlQuery = "DELETE FROM dbusers WHERE id = @id";
            await _dbConn.ExecuteAsync(sqlQuery, new { id });
        }
    }
}
