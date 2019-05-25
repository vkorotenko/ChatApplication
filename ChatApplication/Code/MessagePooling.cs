#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  23.05.2019 11:52
#endregion
using ChatApplication.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApplication.Code
{
    /// <summary>
    /// Класс долгого опроса сообщений
    /// </summary>
    public class MessagePolling
    {
        private static readonly List<MessagePolling> Subscribers = new List<MessagePolling>();

        public static void Publish(int userId, LpMessage message)
        {
            lock (Subscribers)
            {
                var all = Subscribers.ToList();
                foreach (var poll in all)
                {
                    if (poll.UserId == userId) poll.Notify(message);
                }
            }
        }

        private readonly TaskCompletionSource<bool> _taskCompleteion = new TaskCompletionSource<bool>();

        private int UserId { get; set; }
        private LpMessage Message { get; set; }
        /// <summary>
        /// Конструктор долгого опроса.
        /// </summary>
        /// <param name="userId"></param>
        public MessagePolling(int userId)
        {
            this.UserId = userId;
            lock (Subscribers)
            {
                Subscribers.Add(this);
            }
        }

        private void Notify(LpMessage message)
        {
            Message = message;
            _taskCompleteion.SetResult(true);
        }

        public async Task<LpMessage> WaitAsync()
        {
            await Task.WhenAny(_taskCompleteion.Task, Task.Delay(30000)); // blocking wait until event occurs or timeout
            lock (Subscribers)
            {
                Subscribers.Remove(this);
            }
            return this.Message;
        }
    }
}
