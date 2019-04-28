#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  14.04.2019 7:31
#endregion
using System;

namespace ChatApplication.Models
{
    /// <summary>
    /// Загружаемый файл
    /// </summary>
    public class UploadFile
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Имя файла
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Время загрузки
        /// </summary>
        public DateTime Upload { get; set; }
        
        /// <summary>
        /// URL файла
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Путь к файлу
        /// </summary>
        public string Path { get; set; }
    }
}