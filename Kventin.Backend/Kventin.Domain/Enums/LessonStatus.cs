using System.ComponentModel;

namespace Kventin.DataAccess.Enums
{
    public enum LessonStatus
    {
        /// <summary>
        /// Запланировано
        /// </summary>
        [Description("Запланировано")]
        Planned = 0,

        /// <summary>
        /// Проведено
        /// </summary>
        [Description("Проведено")]
        Completed = 1,

        /// <summary>
        /// Отменено
        /// </summary>
        /// Отправляю цифру в запросе
        [Description("Отменено")]
        Canceled = 2,
    }
}
