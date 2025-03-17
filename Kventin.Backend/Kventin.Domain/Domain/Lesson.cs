using Kventin.DataAccess.Domain.Base;
using Kventin.DataAccess.Enums;

namespace Kventin.DataAccess.Domain
{
    /// <summary>
    /// Занятие
    /// </summary>
    public class Lesson : BaseEntity
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
        /// Дата проведения
        /// </summary>
        public DateOnly Date {  get; set; }

        /// <summary>
        /// Тема занятия
        /// </summary>
        public string? Topic { get; set; }

        /// <summary>
        /// Описание занятия
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Статус занятия
        /// </summary>
        public LessonStatus Status { get; set; }

        /// <summary>
        /// Присутствующие ученики
        /// </summary>
        public List<User> StudentsAttended { get; set; } = [];

        /// <summary>
        /// Оценки
        /// </summary>
        public List<Mark> Marks { get; set; } = [];

        /// <summary>
        /// Элемент расписания (шаблон), по которому создано занятие
        /// Может быть null, чтобы была возможность добавлять внеплановые занятия
        /// </summary>
        public ScheduleItem? ScheduleItem { get; set; }
        /// <summary>
        /// Id элемента расписания
        /// </summary>
        public int? ScheduleItemId { get; set; }
    }
}
