using System.ComponentModel;

namespace Kventin.DataAccess.Enums
{
    public enum LessonStatus
    {
        /// <summary>
        /// Запланировано
        /// </summary>
        Planned = 0,
        
        /// <summary>
        /// Проведено
        /// </summary>
        Completed = 1,
        
        /// <summary>
        /// Отменено
        /// </summary>
        Canceled = 2,
    }
}
