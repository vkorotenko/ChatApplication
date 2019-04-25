#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  20.04.2019 11:57
#endregion

using ChatApplication.Dbl.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatApplication.Dbl.Repository
{
    /// <summary>
    /// Репозиторий для сообщений
    /// </summary>
    public interface IMessageRepository
    {
        /// <summary>
        /// Создание элемента
        /// </summary>
        /// <param name="item">Новый элемент</param>
        /// <returns>Возвращает вставленный элемент</returns>
        Task<DbMessage> Create(DbMessage item);
        /// <summary>
        /// Обновление элемента
        /// </summary>
        /// <param name="item">Элемент для обновления</param>
        /// <returns>Задача</returns>
        Task Update(DbMessage item);    
        /// <summary>
        /// Удаление элемента по идентификатору
        /// </summary>
        /// <param name="id">Первичный ключ</param>
        /// <returns>Задача</returns>
        Task Delete(long id);
        /// <summary>
        /// Получаем элемент по идентификатору
        /// </summary>
        /// <param name="id">Первичный ключ</param>
        /// <returns></returns>
        Task<DbMessage> Get(long id);
        /// <summary>
        /// Список всех элементов
        /// </summary>
        /// <returns></returns>
        Task<List<DbMessage>> GetAll();

        /// <summary>
        /// Получение списка сообщений для топика по идентификатору.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<DbMessage>> GetMessagesForTopic(long id);

    }
}