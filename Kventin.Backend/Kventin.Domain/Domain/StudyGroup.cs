using Kventin.DataAccess.Domain.Base;

namespace Kventin.DataAccess.Domain
{
    /// <summary>
    /// Учебная группа
    /// </summary>
    public class StudyGroup : BaseEntity
    {
        /// <summary>
        /// Название группы
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Id предмета
        /// </summary>
        public required int SubjectId { get; set; }

        /// <summary>
        /// Предмет
        /// </summary>
        public required Subject Subject { get; set; }

        /// <summary>
        /// Id преподавателя
        /// </summary>
        public required int TeacherId { get; set; }

        /// <summary>
        /// Преподаватель
        /// </summary>
        public required User Teacher { get; set; }
        
        /// <summary>
        /// Ученики
        /// </summary>
        public List<User> Students { get; set; } = [];

        /// <summary>
        /// Занятия
        /// </summary>
        public List<Lesson> Lessons { get; set; } = [];

        /// <summary>
        /// Занятия в расписании
        /// </summary>
        public List<ScheduleItem> ScheduleItems { get; set; } = [];

        /// <summary>
        /// Полученные задания
        /// </summary>
        public List<Exercise> RecievedExercises { get; set; } = [];
    }
}
