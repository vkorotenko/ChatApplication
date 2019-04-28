#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  28.04.2019 21:28
#endregion

using ChatApplication.Dbl.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatApplication.Dbl.Repository
{
    public interface IArticleRepository
    {                
        /// <summary>
        /// Получение элемента по идентификатору
        /// </summary>
        /// <param name="id">Первичный ключ</param>
        /// <returns></returns>
        Task<DbArticle> Get(int id);
        /// <summary>
        /// Получение всех элементов
        /// </summary>
        /// <returns></returns>
        Task<List<DbArticle>> GetAll();

        /// <summary>
        /// получение сообщений от пользователя
        /// </summary>
        /// <returns></returns>
        Task<List<DbArticle>> GetAllFromUser(int id);
    }
}
