﻿#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  28.04.2019 21:31
#endregion

using ChatApplication.Dbl.Models;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApplication.Dbl.Repository
{
    public class ArticleRepository : IArticleRepository
    {
        readonly IDbConnection _db = null;
        /// <summary>
        /// Репозиторий обьявлений
        /// </summary>
        /// <param name="dbConnection">Соединение с базой</param>
        public ArticleRepository(IDbConnection dbConnection)
        {
            _db = dbConnection;
        }
        /// <summary>
        /// Получение элемента по идентификатору
        /// </summary>
        /// <param name="id">Первичный ключ</param>
        /// <returns></returns>
        public async Task<DbArticle> Get(int id)
        {
            var sql = @"SELECT da.id as id, da.title as title, da.code as code, da.user_id  as userid, dm.name as Vendor
                        FROM admin_zap.article as da
                        INNER JOIN admin_zap.manufacturer as dm
                        ON dm.id = da.manufacture_id
                        WHERE da.id = @id";
            var articles = await _db.QueryAsync<DbArticle>(sql, new { id });
            foreach (var article in articles)
            {
                var aid = article.Id;
                sql = @"SELECT * FROM admin_zap.article_image WHERE article_id =@aid";
                var photos = await _db.QueryAsync<ArticleImage>(sql, new { aid });
                article.Photos = photos;
            }
            return articles.FirstOrDefault();
        }

        /// <summary>
        /// Получение всех элементов
        /// </summary>
        /// <returns></returns>
        public async Task<List<DbArticle>> GetAll()
        {
            var sql = @"SELECT da.id as id, da.title as title, da.code as code, da.user_id  as userid, dm.name as Vendor
                        FROM admin_zap.article as da
                        INNER JOIN admin_zap.manufacturer as dm
                        ON dm.id = da.manufacture_id ";
            return (await _db.QueryAsync<DbArticle>(sql)).ToList();
        }

        /// <summary>
        /// получение сообщений от пользователя
        /// </summary>
        /// <returns></returns>
        public async Task<List<DbArticle>> GetAllFromUser(int id)
        {
            var sql = @"SELECT da.id as id, da.title as title, da.code as code, da.user_id  as userid, dm.name as Vendor
                        FROM admin_zap.article as da
                        INNER JOIN admin_zap.manufacturer as dm
                        ON dm.id = da.manufacture_id 
                        WHERE da.user_id =@id";
            return (await _db.QueryAsync<DbArticle>(sql, new { id })).ToList();
        }

        /// <summary>
        /// Получение цены по номеру обьявления.
        /// </summary>
        /// <param name="id">Идентификатор обьявления</param>
        /// <returns></returns>
        public async Task<int> GetPrice(long id)
        {
            var sql = @"SELECT new_price 
                        FROM admin_zap.article 
                        WHERE id=@id";
            var results = await _db.QueryAsync<int>(sql, new { id });
            var result = results.FirstOrDefault();
            return result;
        }
    }
}