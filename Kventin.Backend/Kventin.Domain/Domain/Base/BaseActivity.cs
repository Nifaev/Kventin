namespace Kventin.DataAccess.Domain.Base
{
    /// <summary>
    /// Базовая сущность для учета рабочих/учебных часов 
    /// </summary>
    public abstract class BaseActivity : BaseEntity
    {
        /// <summary>
        /// Начало периода
        /// </summary>
        public DateOnly PeriodStartDate { get; set; }

        /// <summary>
        /// Конец периода
        /// </summary>
        public DateOnly PeriodEndDate { get; set; }

        /// <summary>
        /// Количество групповых занятий
        /// </summary>
        public int GroupLessonsCount { get; set; }

        /// <summary>
        /// Количество индивидуальных занятий
        /// </summary>
        public int IndividualLessonsCount { get; set; }
    }
}
