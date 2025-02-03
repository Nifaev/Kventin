using Kventin.DataAccess.Domain.Base;

namespace Kventin.DataAccess.Domain
{
    /// <summary>
    /// Предмет
    /// </summary>
    public class Subject : BaseEntity
    {
        /// <summary>
        /// Название предмета
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Занятия
        /// </summary>
        public List<Lesson> Lessons { get; set; } = [];

        /// <summary>
        /// Группы, занимающиеся по предмету
        /// </summary>
        public List<StudyGroup> StudyGroups { get; set; } = [];

        /// <summary>
        /// Занятия в расписании
        /// </summary>
        public List<ScheduleItem> ScheduleItems { get; set; } = [];
    }
}
