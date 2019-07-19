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
        /// <summary>
        /// Получение последнего сообщения в топике.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DbMessage> GetLastMessageForTopic(long id);
            /// <summary>
        /// помечает все сообщения топика как прочтенные
        /// </summary>
        /// <param name="topicid">Топик для очистки</param>
        /// <param name="id">идентификатор пользователя</param>
        /// <returns></returns>
        Task MarkMessagesInTopikAsRead(long topicid, int id);
        
        /// <summary>
        /// Получаем непрочитанные сообщения для пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns></returns>
        Task<List<DbMessage>> GetNewMessagesForUser(int userId);


        /// <summary>
        /// Получение количества непрочтенных писем
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> GetUnreadMessages(int userId);
        /// <summary>
        /// Получение количества непрочтенных писем для администратора или менеджера
        /// </summary>
        /// <param name="id">идентификатор</param>
        /// <returns></returns>
        Task<long> GetUnreadMessagesForAdmin(int id);
        /// <summary>
        /// Получение общего количества писем для пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<long> GetTotalMessages(int userId);

        /// <summary>
        /// Получаем список новых сообщений для администратора.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<DbMessage>> GetNewMessagesForAdmin(int id);

        
    }
}