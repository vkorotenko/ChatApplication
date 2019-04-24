#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  24.04.2019 7:19
#endregion
using Microsoft.Extensions.Logging;

namespace ChatApplication.Code
{
    /// <summary>
    /// Расширение для конфигурации файлового логгера
    /// </summary>
    public static class FileLoggerExtensions
    {
        /// <summary>
        /// Добавление файла лога
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static ILoggerFactory AddFile(this ILoggerFactory factory,
            string filePath)
        {
            factory.AddProvider(new FileLoggerProvider(filePath));
            return factory;
        }
    }
}