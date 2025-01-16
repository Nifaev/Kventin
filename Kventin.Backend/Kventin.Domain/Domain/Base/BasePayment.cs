using Kventin.DataAccess.Enums;

namespace Kventin.DataAccess.Domain.Base
{
    /// <summary>
    /// Базовая сущность платежа
    /// </summary>
    public abstract class BasePayment : BaseEntity
    {
        /// <summary>
        /// Дата и время совершения платежа
        /// </summary>
        public DateTime? PaymentDateTime { get; set; }

        /// <summary>
        /// Сумма платежа
        /// </summary>
        public double Sum { get; set; }

        /// <summary>
        /// Способ оплаты/выплаты
        /// </summary>
        public PaymentMethod Type { get; set; }

        /// <summary>
        /// Был ли оплачен
        /// </summary>
        public bool WasPaid { get; set; }
    }
}
