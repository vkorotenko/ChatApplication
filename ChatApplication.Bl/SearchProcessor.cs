#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  19.07.2019 8:51
#endregion

using System.Collections.Generic;
using System.Linq;
using ChatApplication.Dbl.Models;

namespace ChatApplication.Bl
{
    /// <summary>
    /// Класс для поиска информации в наборе данных
    /// </summary>
    public class SearchProcessor
    {
        IEnumerable<DbTopic> _sorceTopics;
        public SearchProcessor(IEnumerable<DbTopic> topics)
        {
            _sorceTopics = topics;
        }
        /// <summary>
        /// Ищет вхождения поисковой фразы в полях. 
        /// </summary>
        /// <param name="query">Поисковая строка, в случае пустой возвращается исходная коллекция</param>
        /// <remarks>
        /// Поисковые поля: <see cref="DbTopic.Title"/>, <see cref="DbTopic.Vendor"/>, <see cref="DbTopic.VendorCode"/>
        /// </remarks>
        /// <returns></returns>
        public IEnumerable<DbTopic> Contains(string query)
        {
            var result = new List<DbTopic>();
            query = query.ToLower();
            // поиск по пустой строке, все результаты
            if (!string.IsNullOrWhiteSpace(query))
            {

                result = (from t in _sorceTopics
                          where t.Title.ToLower().Contains(query) ||
                                t.Vendor.ToLower().Contains(query) ||
                                t.VendorCode.ToLower().Contains(query) ||
                                t.Name.ToLower().Contains(query) ||
                                t.FullName.ToLower().Contains(query)
                          select t).ToList();
            }
            else
            {
                result = _sorceTopics.ToList();
            }
            return result;
        }
    }
}
