using System.ComponentModel;

namespace Kventin.DataAccess.Enums
{
    public enum MarkValue
    {
        /// <summary>
        /// Не оценено
        /// </summary>
        [Description("Не оценено")]
        NotRated = 1,

        /// <summary>
        /// Неудовлетворительно
        /// </summary>
        [Description("Неудовлетворительно")]
        Unsatisfactory = 2,

        /// <summary>
        /// Удовлетворительно
        /// </summary>
        [Description("Удовлетворительно")]
        Satisfactory = 3,

        /// <summary>
        /// Хорошо
        /// </summary>
        [Description("Хорошо")]
        Good = 4,

        /// <summary>
        /// Отлично
        /// </summary>
        [Description("Отлично")]
        Excellent = 5,
    }
}
