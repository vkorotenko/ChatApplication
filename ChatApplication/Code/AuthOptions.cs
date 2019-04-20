#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  14.04.2019 7:34
#endregion

using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ChatApplication.Code
{
    /// <summary>
    /// Опции для аутентификации
    /// </summary>
    public class AuthOptions
    {
        /// <summary>
        /// Кто выдал
        /// </summary>
        public const string ISSUER = "VKorotenko"; // издатель токена
        /// <summary>
        /// Для кого этот ключ
        /// </summary>
        public const string AUDIENCE = "http://localhost:51884/"; // потребитель токена
        /// <summary>
        /// Пароль шифрования
        /// </summary>
        const string KEY = "dsachjdskjdckgdsdksgshdjgvsdk";   // ключ для шифрации
        /// <summary>
        /// Время жизни токена
        /// </summary>
        public const int LIFETIME = 1; // время жизни токена - 1 минута
        /// <summary>
        /// Получить ключ для шифрования
        /// </summary>
        /// <returns></returns>
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
