#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  20.04.2019 7:53
#endregion

using System.Data;
using ChatApplication.Dbl.Repository;
using MySql.Data.MySqlClient;

namespace ChatApplication.Dbl
{
    /// <summary>
    /// Контекст базы данных, создается при старте приложения, предоставляет доступ ко всем репозиториям.
    /// </summary>
    public class DbContext
    {        
        private readonly IDbConnection _dbConnection;
        private readonly IUserRepository _users;
        private readonly IRoleRepository _roles;
        private readonly IInroleRepository _inrole;
        private readonly ITopicRepository _topics;
        private readonly IMessageRepository _messages;
        private readonly IFileRepository _files;
        public DbContext(string connectionString)
        {            
            _dbConnection = new MySqlConnection(connectionString);
            _users = new UserRepository(_dbConnection);
            _roles = new RoleRepository(_dbConnection);
            _inrole = new InroleRepository(_dbConnection);
            _topics = new TopicRepository(_dbConnection); 
            _messages = new MessageRepository(_dbConnection);
            _files = new FileRepository(_dbConnection);
        }

        /// <summary>
        /// Репозиторий пользователей.
        /// </summary>
        public IUserRepository Users => _users;
        
        /// <summary>
        /// Репозиторий ролей.
        /// </summary>
        public IRoleRepository Roles => _roles;

        /// <summary>
        /// Пользователь в роли.
        /// </summary>
        public IInroleRepository UserInRole => _inrole;
        
        /// <summary>
        /// Топики. 
        /// </summary>
        public ITopicRepository Topics => _topics;
        /// <summary>
        /// Сообщения.
        /// </summary>
        public IMessageRepository Messages => _messages;
        /// <summary>
        /// Файлы.
        /// </summary>
        public IFileRepository Files => _files;
    }
}
