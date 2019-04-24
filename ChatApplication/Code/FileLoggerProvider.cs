#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  24.04.2019 7:16
#endregion

using Microsoft.Extensions.Logging;

namespace ChatApplication.Code
{
    /// <summary>
    /// Провайдер логгирования в файл
    /// </summary>
    public class FileLoggerProvider : ILoggerProvider
    {
        /// <summary>
        /// Путь логгирования
        /// </summary>
        private string _path;
        /// <summary>
        /// Конструктор провайдера логгирования
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        public FileLoggerProvider(string path)
        {
            _path = path;
        }
        /// <summary>
        /// Создание логгера
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(_path);
        }
        /// <summary>
        /// Уничтожение обьектов
        /// </summary>
        public void Dispose()
        {
        }
    }
}
