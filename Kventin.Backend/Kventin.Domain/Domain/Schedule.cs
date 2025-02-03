using Kventin.DataAccess.Domain.Base;

namespace Kventin.DataAccess.Domain
{
    /// <summary>
    /// Расписание
    /// </summary>
    public class Schedule : BaseEntity
    {
        /// <summary>
        /// Начало действия расписания
        /// </summary>
        public int StartYear { get; set; }

        /// <summary>
        /// Конец действия расписания
        /// </summary>
        public int EndYear { get; set; }

        /// <summary>
        /// Элементы расписания (занятия)
        /// </summary>
        public List<ScheduleItem> Items { get; set; } = [];
    }
}
