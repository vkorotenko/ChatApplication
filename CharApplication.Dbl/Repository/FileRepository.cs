#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  20.04.2019 16:33
#endregion
using ChatApplication.Dbl.Models;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApplication.Dbl.Repository
{
    public class FileRepository : IFileRepository
    {
        readonly IDbConnection _dbConn = null;
        /// <summary>
        /// Репозиторий сообщений
        /// </summary>
        /// <param name="dbConnection">Соединение с базой</param>
        public FileRepository(IDbConnection dbConnection)
        {
            _dbConn = dbConnection;
        }
        /// <summary>
        /// Создание элемента
        /// </summary>
        /// <param name="item">Элемент</param>
        /// <returns>Созданный элемент</returns>
        public async Task<DbFile> Create(DbFile item)
        {            
            var sqlQuery = @"INSERT INTO dbfiles (
                            `id`,                            
                            `name`,
                            `authorid`,
                            `created`,
                            `messageid`) 
                            VALUES( 
                            @Id,
                            @Name,                             
                            @AuthorId, 
                            @Created, 
                            @MessageId ); 
                            
                            SELECT LAST_INSERT_ID()";
            long? userId = (await _dbConn.QueryAsync<long>(sqlQuery, item)).FirstOrDefault();
            item.Id = userId.Value;
            return item;
        }
        /// <summary>
        /// Обновление элемента
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task Update(DbFile item)
        {
            const string sqlQuery = @"UPDATE dbfiles
                                        SET
                                        `id` = @Id,
                                        `name` = @Name,                                        
                                        `authorid` = @AuthorId,
                                        `created` = @Created,
                                        `messageid` = @MessageId                                        
                                        WHERE `id` = @Id;";
            await _dbConn.ExecuteAsync(sqlQuery, item);
        }

        /// <summary>
        /// Удаление элемента
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        public async Task Delete(long id)
        {
            const string sqlQuery = "DELETE FROM dbfiles WHERE id = @id";
            await _dbConn.ExecuteAsync(sqlQuery, new { id });
        }

        /// <summary>
        /// Получение элемента по идентификатору
        /// </summary>
        /// <param name="id">Первичный ключ</param>
        /// <returns></returns>
        public async Task<DbFile> Get(long id)
        {
            return (await _dbConn.QueryAsync<DbFile>("SELECT * FROM dbfiles WHERE id = @id", new { id })).FirstOrDefault();
        }

        /// <summary>
        /// Получение всех элементов
        /// </summary>
        /// <returns></returns>
        public async Task<List<DbFile>> GetAll()
        {
            return (await _dbConn.QueryAsync<DbFile>("SELECT * FROM dbfiles")).ToList();
        }        
    }
}