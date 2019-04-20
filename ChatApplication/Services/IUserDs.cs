using System.Threading.Tasks;
using ChatApplication.Poco;

namespace ChatApplication.Services
{
    /// <summary>
    /// Методы расширяющие пользовательский датасорс
    /// </summary>
    interface IUserDs
    {
        /// <summary>
        /// Получение пользователя по email
        /// </summary>
        /// <returns></returns>
        Task<ApplicationUser> GetItemByPhone(string email);
    }
}
