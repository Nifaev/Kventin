using System.ComponentModel;

namespace Kventin.DataAccess.Enums
{
    public enum DayOfTheWeek
    {
        /// <summary>
        /// Понедельник
        /// </summary>
        [Description("Понедельник")]
        Monday = 1,

        /// <summary>
        /// Вторник
        /// </summary>
        [Description("Вторник")]
        Tuesday = 2,

        /// <summary>
        /// Среда
        /// </summary>
        [Description("Среда")]
        Wednesday = 3,

        /// <summary>
        /// Четверг
        /// </summary>
        [Description("Четверг")]
        Thursday = 4,

        /// <summary>
        /// Пятница
        /// </summary>
        [Description("Пятница")]
        Friday = 5,

        /// <summary>
        /// Суббота
        /// </summary>
        [Description("Суббота")]
        Saturday = 6,

        /// <summary>
        /// Воскресенье
        /// </summary>
        [Description("Воскресенье")]
        Sunday = 7,
    }
}
