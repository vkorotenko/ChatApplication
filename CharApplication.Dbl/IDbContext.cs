#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  22.04.2019 16:41
#endregion
using ChatApplication.Dbl.Repository;

namespace ChatApplication.Dbl
{
    public interface IDbContext
    {
        /// <summary>
        /// Репозиторий пользователей.
        /// </summary>
        IUserRepository Users { get; }

        /// <summary>
        /// Репозиторий ролей.
        /// </summary>
        IRoleRepository Roles { get; }

        /// <summary>
        /// Пользователь в роли.
        /// </summary>
        IInroleRepository UserInRole { get; }

        /// <summary>
        /// Топики. 
        /// </summary>
        ITopicRepository Topics { get; }

        /// <summary>
        /// Сообщения.
        /// </summary>
        IMessageRepository Messages { get; }

        /// <summary>
        /// Файлы.
        /// </summary>
        IFileRepository Files { get; }
        /// <summary>
        /// Репозиторий сообщений
        /// </summary>
        IArticleRepository Articles { get; }
    }
}