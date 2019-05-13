#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  13.05.2019 6:35
#endregion


namespace ChatApplication.Dbl.Models
{
    /// <summary>
    /// Картинки для обьявления.
    /// </summary>
    public class ArticleImage
    {
        public int Id { get; set; }

        public int ArticleId { get; set; }

        public string Photo { get; set; }
    }
}
