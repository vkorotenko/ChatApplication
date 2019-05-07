#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  20.04.2019 9:59
#endregion

using System;
using ChatApplication.Dbl.Models;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApplication.Dbl.Repository
{
    public class TopicRepository : ITopicRepository
    {
        readonly IDbConnection _dbConn = null;
        public TopicRepository(IDbConnection dbConnection)
        {
            _dbConn = dbConnection;
        }
        /// <summary>
        /// Создание топика
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<DbTopic> Create(DbTopic item)
        {
            var sqlQuery = @"INSERT INTO dbtopics (id, title, announcementid, vendor, vendorcode, authorid, created) 
VALUES(@Id, @Title, @AnnouncementId, @Vendor, @VendorCode, @AuthorId, @Created); 
SELECT LAST_INSERT_ID()";
            long? id = (await _dbConn.QueryAsync<long>(sqlQuery, item)).FirstOrDefault();
            item.Id = id.Value;
            return item;
        }

        /// <summary>
        /// Удаление топика
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Delete(long id)
        {
            const string sqlQuery = "DELETE FROM dbtopics WHERE id = @id";
            await _dbConn.ExecuteAsync(sqlQuery, new { id });
        }

        /// <summary>
        /// Получение топика по элементу
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DbTopic> Get(long id)
        {
            return (await _dbConn.QueryAsync<DbTopic>("SELECT * FROM dbtopics WHERE id = @id", new { id })).FirstOrDefault();
        }

        /// <summary>
        /// Получение всех элементов
        /// </summary>
        /// <returns></returns>
        public async Task<List<DbTopic>> GetAll()
        {
            return (await _dbConn.QueryAsync<DbTopic>("SELECT * FROM dbtopics ORDER BY updated DESC")).ToList();
        }

        /// <summary>
        /// Обновление элемента
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task Update(DbTopic user)
        {

            const string sqlQuery = @"UPDATE dbtopics
                                        SET
                                        title = @Title,
                                        announcementid = @AnnouncementId,
                                        vendor = @Vendor,
                                        vendorcode = @VendorCode,
                                        authorid = @AuthorId,
                                        created = @Created,
                                        updated = @Updated
                                        WHERE id = @Id";
            await _dbConn.ExecuteAsync(sqlQuery, user);
        }

        /// <summary>
        /// Обновляем время последнего доступа к топику.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task UpdateTs(long id)
        {
            const string sqlQuery = @"UPDATE dbtopics
                                        SET                                        
                                        updated = @Updated
                                        WHERE id = @id";
            var updated = DateTime.Now;
            await _dbConn.ExecuteAsync(sqlQuery, new { id, updated });
        }

        /// <summary>
        /// Получение топиков по идентификатору пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <returns></returns>
        public async Task<List<DbTopic>> GetByUserId(int id)
        {
            var sql = @"SELECT id,title,announcementid, vendor, vendorcode,authorid,created, updated, 
                        (SELECT COUNT(dbm.id) FROM dbmessages as dbm WHERE dbt.id = dbm.topicid AND dbm.isread = 0) as Unread
                          FROM dbtopics as dbt
                          WHERE dbt.authorid = @id
                          ORDER BY updated DESC";
            var res = await _dbConn.QueryAsync<DbTopic>(sql, new { id });
            return res.ToList();
        }

        /// <summary>
        /// Получение топиков по идентификатору административного пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<DbTopic>> GetByAdminId(int id)
        {
            var sql = @"SELECT id,title,announcementid, vendor, vendorcode,authorid,created, updated, 
                        (SELECT COUNT(dbm.id) FROM dbmessages as dbm WHERE dbt.id = dbm.topicid AND dbm.isread = 0) as Unread
                          FROM dbtopics as dbt                          
                          ORDER BY updated DESC";
            var res = await _dbConn.QueryAsync<DbTopic>(sql, new { id });
            return res.ToList();
        }
    }
}