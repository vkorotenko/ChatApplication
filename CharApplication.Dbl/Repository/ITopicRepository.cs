﻿#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  20.04.2019 9:28
#endregion


using ChatApplication.Dbl.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatApplication.Dbl.Repository
{
    /// <summary>
    /// Репозиторий для запроса топиков
    /// </summary>
    public interface ITopicRepository
    {
        /// <summary>
        /// Создание топика
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<DbTopic> Create(DbTopic item);
        /// <summary>
        /// Удаление топика
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(long id);
        /// <summary>
        /// Получение топика по элементу
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DbTopic> Get(long id);
        /// <summary>
        /// Получение всех элементов
        /// </summary>
        /// <returns></returns>
        Task<List<DbTopic>> GetAll();
        /// <summary>
        /// Обновление элемента
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task Update(DbTopic user);
        /// <summary>
        /// Обновляем время последнего доступа к топику.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task UpdateTs(long id);
        /// <summary>
        /// Получение топиков по идентификатору пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <returns></returns>
        Task<List<DbTopic>> GetByUserId(int id);
        /// <summary>
        /// Получение топиков по идентификатору административного пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<DbTopic>> GetByAdminId(int userId);
    }
}
