using Kventin.DataAccess.Enums;

namespace Kventin.DataAccess.Domain.Base
{
    /// <summary>
    /// Базовый класс для сообщения и уведомления
    /// </summary>
    public abstract class BaseNotification : BaseEntity
    {
        /// <summary>
        /// Статус сообщения/уведомления
        /// </summary>
        public SendStatus Status { get; set; }

        /// <summary>
        /// Содержание
        /// </summary>
        public required string Content { get; set; }

        /// <summary>
        /// Id получателя
        /// </summary>
        public int RecieverId { get; set; }

        /// <summary>
        /// Получатель
        /// </summary>
        public required User Reciever { get; set; }
    }
}
