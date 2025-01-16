using Kventin.DataAccess.Domain.Base;

namespace Kventin.DataAccess.Domain
{
    /// <summary>
    /// Ответ на задание
    /// </summary>
    public class ExerciseAnswer : BaseEntity
    {
        /// <summary>
        /// Содержание
        /// </summary>
        public required string Content { get; set; }

        /// <summary>
        /// Id ученика
        /// </summary>
        public int StudentId { get; set; }

        /// <summary>
        /// Ученик
        /// </summary>
        public required User Student { get; set; }

        /// <summary>
        /// Id задания
        /// </summary>
        public int ExerciseId { get; set; }

        /// <summary>
        /// Задание
        /// </summary>
        public required Exercise Exercise { get; set; }
    }
}
