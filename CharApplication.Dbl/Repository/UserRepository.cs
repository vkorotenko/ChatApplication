#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  19.04.2019 22:06
#endregion
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
            return (await _dbConn.QueryAsync<DbUser>("SELECT * FROM dbusers")).ToList();
        }

        public async Task<DbUser> Get(long id)
        {

            return (await _dbConn.QueryAsync<DbUser>("SELECT * FROM dbusers WHERE id = @id", new { id })).FirstOrDefault();

        }

        public async Task<DbUser> Create(DbUser user)
        {

            // var sqlQuery = "INSERT INTO Users (Name, Age) VALUES(@Name, @Age)";
            // await  db.ExecuteAsync(sqlQuery, user);

            // если мы хотим получить id добавленного пользователя

            var sqlQuery = @"INSERT INTO dbusers (id, name, lastname, middlename, email, password, lastactive) 
VALUES(@Id, @Name, @LastName, @MiddleName, @Email, @Password, @LastActive); 
SELECT LAST_INSERT_ID()";
            long? userId = (await _dbConn.QueryAsync<long>(sqlQuery, user)).FirstOrDefault();
            user.Id = userId.Value;
            return user;
        }


        public async Task Update(DbUser user)
        {

            const string sqlQuery = @"UPDATE dbusers
SET
`id` = @Id,
`name` = @Name,
`lastname` = @LastName,
`middlename` = @MiddleName,
`email` = @Email,
`password` = @Password,
`lastactive` = @LastActive
WHERE `id` = @Id ;";
            await _dbConn.ExecuteAsync(sqlQuery, user);

        }

        public async Task Delete(long id)
        {
            const string sqlQuery = "DELETE FROM dbusers WHERE id = @id";
            await _dbConn.ExecuteAsync(sqlQuery, new { id });
        }
    }
}
