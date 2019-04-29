#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  20.04.2019 16:30
#endregion

using ChatApplication.Dbl.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatApplication.Dbl.Repository
{
    public interface IFileRepository
    {
        /// <summary>
        /// Создание элемента
        /// </summary>
        /// <param name="item">Элемент</param>
        /// <returns>Созданный элемент</returns>
        Task<DbFile> Create(DbFile item);
        /// <summary>
        /// Удаление элемента
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        Task Delete(long id);
        /// <summary>
        /// Получение элемента по идентификатору
        /// </summary>
        /// <param name="id">Первичный ключ</param>
        /// <returns></returns>
        Task<DbFile> Get(long id);
        /// <summary>
        /// Получение всех элементов
        /// </summary>
        /// <returns></returns>
        Task<List<DbFile>> GetAll();
        /// <summary>
        /// Обновление элемента
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task Update(DbFile item);

        /// <summary>
        /// Получение аттача для сообщения.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DbFile> GetForMessage(long id);
    }
}
