using Kventin.DataAccess.Domain.Base;

namespace Kventin.DataAccess.Domain
{
    /// <summary>
    /// Учебная группа
    /// </summary>
    public class StudyGroup : BaseEntity
    {
        /// <summary>
        /// Id предмета
        /// </summary>
        public int SubjectId { get; set; }

        /// <summary>
        /// Предмет
        /// </summary>
        public required Subject Subject { get; set; }
        
        /// <summary>
        /// Ученики
        /// </summary>
        public List<User> Students { get; set; } = [];

        /// <summary>
        /// Занятия
        /// </summary>
        public List<Lesson> Lessons { get; set; } = [];

        /// <summary>
        /// Полученные задания
        /// </summary>
        public List<Exercise> RecievedExercises { get; set; } = [];
    }
}
