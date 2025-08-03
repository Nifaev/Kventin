using Kventin.DataAccess.Domain.Base;

namespace Kventin.DataAccess.Domain
{
    /// <summary>
    /// Учет учебных часов
    /// </summary>
    public class StudentActivity : BaseActivity
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public int StudentId { get; set; }

        /// <summary>
        /// Ученик
        /// </summary>
        public required User Student { get; set; }

        /// <summary>
        /// Id тарифа обучения
        /// </summary>
        public int TariffId { get; set; }

        /// <summary>
        /// Тариф обучения
        /// </summary>
        public required TuitionTariff Tariff { get; set; }
    }
}
