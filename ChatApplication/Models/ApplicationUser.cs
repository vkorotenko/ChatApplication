﻿#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  14.04.2019 7:04
#endregion

using System.Collections.Generic;
using ChatApplication.Dbl.Models;
using Newtonsoft.Json;

namespace ChatApplication.Models
{
    /// <inheritdoc />
    /// <summary>
    /// Модель пользователя в приложении
    /// </summary>    
    public class ApplicationUser : BaseUser
    {        
        /// <summary>
        /// Общее количество новых сообщений.
        /// </summary>
        [JsonProperty("messages")]
        public int NewMessages { get; set; }
        /// <summary>
        /// Топики открытые пользователем
        /// </summary>
        [JsonProperty("topics")]
        public IEnumerable<DbTopic> Topics { get; set; }
    }
}
