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
        /// Помечает все сообщения топика как прочтенные
        /// </summary>
        /// <param name="topicid"></param>
        /// <returns></returns>
        public async Task MarkMessagesInTopikAsRead(long topicid)
        {
            var sql = @"UPDATE dbmessages
                            SET
                            isread = 1
                            WHERE topicid = @topicid";
            await _dbConn.QueryAsync<DbMessage>(sql, new { topicid });
        }
    }
}