#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  24.04.2019 7:11
#endregion

using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;

namespace ChatApplication.Code
{
    /// <summary>
    /// Файловый логгер
    /// </summary>
    public class FileLogger : ILogger
    {
        /// <summary>
        /// Путь к файлу
        /// </summary>
        private string _filePath;
        private static object _lock = new object();
        /// <summary>
        /// Логгер
        /// </summary>
        /// <param name="path">Путь к логгеру</param>
        public FileLogger(string path)
        {
            _filePath = path;
        }
        /// <summary>
        /// Создание логгера и выделение ресурсов
        /// </summary>
        /// <typeparam name="TState">Состояние</typeparam>
        /// <param name="state">Значение</param>
        /// <returns></returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
        /// <summary>
        /// Флаг разрешения логгирования
        /// </summary>
        /// <param name="logLevel">Уровень логгирования</param>
        /// <returns></returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            //return logLevel == LogLevel.Trace;
            return true;
        }
        /// <summary>
        /// Метод для логгирования
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel"></param>
        /// <param name="eventId"></param>
        /// <param name="state"></param>
        /// <param name="exception"></param>
        /// <param name="formatter"></param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                lock (_lock)
                {
                    try
                    {
                        File.AppendAllText(_filePath, formatter(state, exception) + Environment.NewLine);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Debug.WriteLine("UserName: {0}", Environment.UserName);
                        Debug.WriteLine(ex);
                    }
                }
            }
        }
    }
}
