using Kventin.DataAccess.Domain.Base;
using Kventin.DataAccess.Enums;

namespace Kventin.DataAccess.Domain
{
    /// <summary>
    /// Элемент раписания (абстрактное занятие)
    /// </summary>
    public class ScheduleItem : BaseEntity
    {
        /// <summary>
        /// Время начала
        /// </summary>
        public TimeOnly StartTime { get; set; }

        /// <summary>
        /// Время окончания
        /// </summary>
        public TimeOnly EndTime { get; set; }

        /// <summary>
        /// День недели
        /// </summary>
        public DayOfTheWeek DayOfWeek { get; set; }

        /// <summary>
        /// Является ли онлайн-занятием
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// Кабинет
        /// </summary>
        public string? Classroom { get; set; }

        /// <summary>
        /// Id предмета
        /// </summary>
        public int SubjectId { get; set; }

        /// <summary>
        /// Предмет
        /// </summary>
        public required Subject Subject { get; set; }

        /// <summary>
        /// Id группы
        /// </summary>
        public int StudyGroupId { get; set; }

        /// <summary>
        /// Группа
        /// </summary>
        public required StudyGroup StudyGroup { get; set; }

        /// <summary>
        /// Id преподавателя
        /// </summary>
        public int TeacherId { get; set; }

        /// <summary>
        /// Преподаватель
        /// </summary>
        public required User Teacher { get; set; }

        /// <summary>
        /// Id расписания, к которому относится элемент
        /// </summary>
        public int ScheduleId { get; set; }
        
        /// <summary>
        /// Расписание, к которому относится элемент 
        /// </summary>
        public required Schedule Schedule { get; set; }

        /// <summary>
        /// Занятия, созданные по данному "шаблону" (элементу расписания)
        /// </summary>
        public List<Lesson> Lessons { get; set; } = [];
    }
}
