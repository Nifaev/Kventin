using Kventin.DataAccess.Domain.Base;

namespace Kventin.DataAccess.Domain
{
    /// <summary>
    /// Тариф обучения
    /// </summary>
    public class TuitionTariff : BaseEntity
    {
        /// <summary>
        /// Название
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Стоимость
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Количество занятий
        /// </summary>
        public int LessonsCount { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Являются ли занятия индивидуальными
        /// </summary>
        public bool IsIndividual { get; set; }

        /// <summary>
        /// Учеты учебных часов по этому тарифу
        /// </summary>
        public List<StudentActivity> StudentActivities { get; set; } = [];

        /// <summary>
        /// Ученики, обучающиеся по данному тарифу
        /// </summary>
        public List<User> Students { get; set; } = [];
    }
}
