using Kventin.DataAccess.Domain.Base;

namespace Kventin.DataAccess.Domain
{
    /// <summary>
    /// Ставка сотрудника
    /// </summary>
    public class EmployeeRate : BaseEntity
    {
        /// <summary>
        /// Почасовая ставка
        /// </summary>
        public double HourlyRate { get; set; }

        /// <summary>
        /// Ставка за индивидуальное занятие
        /// </summary>
        public double IndividualLessonRate { get; set; }

        /// <summary>
        /// Ставка за групповое занятие
        /// </summary>
        public double GroupLessonRate { get; set; }

        /// <summary>
        /// Ставка за количество учеников на групповом занятии
        /// </summary>
        public double GroupLessonStudentsRate {  get; set; } 

        /// <summary>
        /// Id сотрудника
        /// </summary>
        public long EmployeeId { get; set; }

        /// <summary>
        /// Сотрудник
        /// </summary>
        public required User Employee {  get; set; }
    }
}
