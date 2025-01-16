using Kventin.DataAccess.Domain.Base;
using Kventin.DataAccess.Enums;

namespace Kventin.DataAccess.Domain
{
    /// <summary>
    /// Оценка
    /// </summary>
    public class Mark : BaseEntity
    {
        /// <summary>
        /// Комментарий преподавателя
        /// </summary>
        public required string Comment { get; set; }

        /// <summary>
        /// Непосредственно оценка
        /// </summary>
        public required MarkValue Value { get; set; }

        /// <summary>
        /// Id ученика
        /// </summary>
        public int StudentId { get; set; }

        /// <summary>
        /// Ученик
        /// </summary>
        public required User Student { get; set; }

        /// <summary>
        /// Id преподавателя
        /// </summary>
        public int TeacherId { get; set; }

        /// <summary>
        /// Преподаватель
        /// </summary>
        public required User Teacher { get; set; }

        /// <summary>
        /// Id Занятия
        /// </summary>
        public int? LessonId { get; set; }

        /// <summary>
        /// Занятие
        /// </summary>
        public Lesson? Lesson { get; set; }

        /// <summary>
        /// Id занятия
        /// </summary>
        public int? ExerciseId { get; set; }

        /// <summary>
        /// Занятие
        /// </summary>
        public Exercise? Exercise { get; set; }
    }
}
