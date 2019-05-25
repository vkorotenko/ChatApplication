#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  20.04.2019 12:03
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
    /// Репозиторий для получения сообщений
    /// </summary>
    public class MessageRepository : IMessageRepository
    {
        readonly IDbConnection _dbConn = null;
        /// <summary>
        /// Репозиторий сообщений
        /// </summary>
        /// <param name="dbConnection">Соединение с базой</param>
        public MessageRepository(IDbConnection dbConnection)
        {
            _dbConn = dbConnection;
        }
        /// <summary>
        /// Создание элемента
        /// </summary>
        /// <param name="item">Новый элемент</param>
        /// <returns>Возвращает вставленный элемент</returns>
        public async Task<DbMessage> Create(DbMessage item)
        {
            var sqlQuery = @"INSERT INTO dbmessages (
                            id,
                            `body`,
                            `created`,
                            `authorid`,
                            `isread`,
                            `topicid`) 
                            VALUES( 
                            @Id,
                            @Body, 
                            @Created, 
                            @AuthorId, 
                            @IsRead, 
                            @TopicId); 
                            
                            SELECT LAST_INSERT_ID()";
            long? id = (await _dbConn.QueryAsync<long>(sqlQuery, item)).FirstOrDefault();
            item.Id = id.Value;
            return item;
        }

        /// <summary>
        /// Обновление элемента
        /// </summary>
        /// <param name="item">Элемент для обновления</param>
        /// <returns>Задача</returns>
        public async Task Update(DbMessage item)
        {
            const string sqlQuery = @"UPDATE dbmessages
                                        SET
                                        `id` = @Id,
                                        `body` = @Body,
                                        `created` = @Created,
                                        `authorid` = @AuthorId,
                                        `isread` = @IsRead,
                                        `topicid` = @TopicId
                                        WHERE `id` = @Id ;";
            await _dbConn.ExecuteAsync(sqlQuery, item);
        }

        /// <summary>
        /// Удаление элемента по идентификатору
        /// </summary>
        /// <param name="id">Первичный ключ</param>
        /// <returns>Задача</returns>
        public async Task Delete(long id)
        {
            const string sqlQuery = "DELETE FROM dbmessages WHERE id = @id";
            await _dbConn.ExecuteAsync(sqlQuery, new { id });
        }

        /// <summary>
        /// Получаем элемент по идентификатору
        /// </summary>
        /// <param name="id">Первичный ключ</param>
        /// <returns></returns>
        public async Task<DbMessage> Get(long id)
        {
            return (await _dbConn.QueryAsync<DbMessage>("SELECT * FROM dbmessages WHERE id = @id", new { id })).FirstOrDefault();
        }

        /// <summary>
        /// Список всех элементов
        /// </summary>
        /// <returns></returns>
        public async Task<List<DbMessage>> GetAll()
        {
            return (await _dbConn.QueryAsync<DbMessage>("SELECT * FROM dbmessages")).ToList();
        }

        /// <summary>
        /// Получение списка сообщений для топика по идентификатору.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<DbMessage>> GetMessagesForTopic(long id)
        {
            return (await _dbConn.QueryAsync<DbMessage>("SELECT * FROM dbmessages WHERE topicid= @id ORDER BY created ASC", new { id })).ToList();
        }

        /// <summary>
        /// Получение последнего сообщения в топике.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DbMessage> GetLastMessageForTopic(long id)
        {
            var sql = @"SELECT * FROM dbmessages
                        WHERE topicid = @id
                        ORDER BY created DESC
                        LIMIT 1";
            var res = await _dbConn.QueryAsync<DbMessage>(sql, new {id});
            return res.FirstOrDefault();
        }

        /// <summary>
        /// Помечает все сообщения топика как прочтенные
        /// </summary>
        /// <param name="topicid"></param>
        /// <param name="id">номер пользователя</param>
        /// <returns></returns>
        public async Task MarkMessagesInTopikAsRead(long topicid, int id)
        {
            var sql = @"UPDATE dbmessages
                            SET
                            isread = 1
                            WHERE topicid = @topicid and authorid != @id";
            await _dbConn.QueryAsync<DbMessage>(sql, new { topicid, id });
        }

        /// <summary>
        /// Получаем непрочитанные сообщения для пользователя.
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <returns></returns>
        public async Task<List<DbMessage>> GetNewMessagesForUser(int id)
        {
            var sql = @"SELECT *
                          FROM dbmessages
                         WHERE topicid IN (SELECT id
                                           FROM dbtopics
                                          WHERE authorid =@id) AND authorid != @id AND NOT isread";
            return (await _dbConn.QueryAsync<DbMessage>(sql, new { id })).ToList();
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
                                          WHERE authorid=@userId) AND NOT isread AND authorid != @userId";


            var results = await _dbConn.QueryAsync<long>(sql, new { userId });
            var result = results.FirstOrDefault();
            return result;
        }

        /// <summary>
        /// Получение количества непрочтенных писем для администратора или менеджера
        /// </summary>
        /// <param name="id">идентификатор</param>
        /// <returns></returns>
        public async Task<long> GetUnreadMessagesForAdmin(int id)
        {
            var sql = @"SELECT count(isread) FROM dbmessages
                 WHERE topicid IN (SELECT id
                                           FROM dbtopics
                                          ) AND authorid != @id AND NOT isread";

            var results = await _dbConn.QueryAsync<long>(sql, new { id });
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

        /// <summary>
        /// Получаем список новых сообщений для администратора.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<DbMessage>> GetNewMessagesForAdmin(int id)
        {
            var sql = @"SELECT * FROM dbmessages
                 WHERE topicid IN (SELECT id
                                           FROM dbtopics
                                          ) AND authorid != @id AND NOT isread";

            return (await _dbConn.QueryAsync<DbMessage>(sql, new { id })).ToList();
        }
    }
}