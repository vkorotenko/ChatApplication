#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  28.04.2019 21:22
#endregion


namespace ChatApplication.Dbl.Models
{
    /// <summary>
    /// Модель обьявлений
    /// </summary>
    public class DbArticle
    {
        /*
        SELECT da.id as id, da.title as title, da.code as code, da.user_id  as userid, dm.name as Vendor
            FROM admin_zap.article as da
            INNER JOIN admin_zap.manufacturer as dm
            ON dm.id = da.manufacture_id
            WHERE da.user_id = 52
            */
        /// <summary>
        /// Идентификатор обьявления
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        ////Название обьявления
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Код, вендора
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Автор
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Производитель
        /// </summary>
        public string Vendor { get; set; }
    }
}
