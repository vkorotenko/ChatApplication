#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  18.04.2019 8:52
#endregion
using ChatApplication.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatApplication.Poco;

namespace ChatApplication.Services
{
    /// <summary>
    /// Дата сервис для доступа к данным
    /// </summary>
    public class CompanionDataService : IDataStore<Companion>
    {
        /// <summary>
        /// Добавить элемент
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<bool> AddItemAsync(Companion item)
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Обновить элемент
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<bool> UpdateItemAsync(Companion item)
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Удалить элемент
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteItemAsync(string id)
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Получить элемент по первичному ключу
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Companion> GetItemAsync(string id)
        {
            throw new System.NotImplementedException();
        }
        
        /// <summary>
        /// Получить элементы 
        /// </summary>
        /// <param name="forceRefresh"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Companion>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new System.NotImplementedException();
        }
    }
}