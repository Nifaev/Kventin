using Kventin.DataAccess.Domain.Base;
using Kventin.DataAccess.Enums;
using System.ComponentModel;

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
        /// Дата проведения
        /// </summary>
        public DateOnly? Date {  get; set; }

        /// <summary>
        /// Является ли онлайн-занятием
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// Кабинет
        /// </summary>
        public string? Classroom { get; set; }

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
        /// Присутствующие ученики
        /// </summary>
        public List<User> StudentsAttended { get; set; } = [];

        /// <summary>
        /// Оценки
        /// </summary>
        public List<Mark> Marks { get; set; } = [];

        /// <summary>
        /// Id предмета
        /// </summary>
        public int SubjectId { get; set; }

        /// <summary>
        /// Предмет
        /// </summary>
        public required Subject Subject { get; set; }

        /// <summary>
        /// Является ли занятие абстрактным (то есть элементом расписания)
        /// </summary>
        public bool IsAbstract { get; set; }
    }
}
