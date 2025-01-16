using Kventin.DataAccess.Domain.Base;

namespace Kventin.DataAccess.Domain
{   
    /// <summary>
    /// Задание
    /// </summary>
    public class Exercise : BaseEntity
    {
        /// <summary>
        /// Срок сдачи
        /// </summary>
        public DateTime? DeadlineDateTime { get; set; }

        /// <summary>
        /// Содержание 
        /// </summary>
        public required string Content { get; set; }

        /// <summary>
        /// Индивидуальное
        /// </summary>
        public required bool IsIndividual { get; set; }

        /// <summary>
        /// Id преподавателя
        /// </summary>
        public int TeacherId { get; set; }

        /// <summary>
        /// Преподаватель
        /// </summary>
        public required User Teacher {  get; set; }

        /// <summary>
        /// Id группы
        /// </summary>
        public int StudyGroupId { get; set; }

        /// <summary>
        /// Группа
        /// </summary>
        public required StudyGroup StudyGroup { get; set; }

        /// <summary>
        /// Прикрепленные ответы учеников
        /// </summary>
        public List<ExerciseAnswer> Answers { get; set; } = [];

        /// <summary>
        /// Выставленные оценки
        /// </summary>
        public List<Mark> Marks { get; set; } = [];
    }
}
