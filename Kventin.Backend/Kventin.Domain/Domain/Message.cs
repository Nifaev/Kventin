using Kventin.DataAccess.Domain.Base;

namespace Kventin.DataAccess.Domain
{
    /// <summary>
    /// Сообщение
    /// </summary>
    public class Message : BaseNotification
    {
        /// <summary>
        /// Id отправителя
        /// </summary>
        public long SenderId { get; set; }

        /// <summary>
        /// Отправитель
        /// </summary>
        public required User Sender { get; set; }
    }
}
