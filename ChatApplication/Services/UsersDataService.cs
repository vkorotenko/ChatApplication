#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  18.04.2019 8:34
#endregion

using ChatApplication.Poco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApplication.Dbl;
using ChatApplication.Dbl.Models;

namespace ChatApplication.Services
{
    /// <summary>
    /// Обьект для получения пользователей
    /// </summary>
    public class UsersDataService : IDataStore<ApplicationUser>, IUserDs
    {
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public UsersDataService()
        {
        }
        /// <summary>
        /// Добавление элемента
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<bool> AddItemAsync(ApplicationUser item)
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Обновление элемента
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<bool> UpdateItemAsync(ApplicationUser item)
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Удаление элемента
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteItemAsync(string id)
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Получение элемента по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApplicationUser> GetItemAsync(string id)
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Получение всех элементов
        /// </summary>
        /// <param name="forceRefresh"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ApplicationUser>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Получение по email
        /// </summary>
        /// <returns></returns>
        public async Task<ApplicationUser> GetItemByPhone(string phone)
        {
            DbUser u;
            await Task.Run(() => u = _db.Users.FirstOrDefault(x => x.Phone.ToString() == phone));            
            return u;

        }
    }
}
