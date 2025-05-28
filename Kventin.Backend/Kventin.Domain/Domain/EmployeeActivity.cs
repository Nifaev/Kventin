using Kventin.DataAccess.Domain.Base;

namespace Kventin.DataAccess.Domain
{
    /// <summary>
    /// Учет рабочих часов (Деятельность сотрудника)
    /// </summary>
    public class EmployeeActivity : BaseActivity
    {
        /// <summary>
        /// Суммарное количество учеников на групповых занятиях
        /// </summary>
        public int TotalGroupLessonStudentsCount { get; set; }

        /// <summary>
        /// Id сотрудника
        /// </summary>
        public long EmployeeId { get; set; }

        /// <summary>
        /// Сотрудник
        /// </summary>
        public required User Employee { get; set; }
    }
}
