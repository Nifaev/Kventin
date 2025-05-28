using Kventin.DataAccess.Domain.Base;

namespace Kventin.DataAccess.Domain
{
    /// <summary>
    /// Оплата занятий
    /// </summary>
    public class TuitionPayment : BasePayment
    {
        /// <summary>
        /// Номер договора
        /// </summary>
        public string? ContractBumber { get; set; }

        /// <summary>
        /// Id плательщика (ученик/родитель)
        /// </summary>
        public long PayerId { get; set; }

        /// <summary>
        /// Плательщик (ученик/родитель)
        /// </summary>
        public required User Payer {  get; set; }
    }
}
