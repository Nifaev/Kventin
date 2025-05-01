using Kventin.DataAccess.Enums;

namespace Kventin.Services.Infrastructure.Extensions
{
    public static class DayOfTheWeekExtensions
    {
        /// <summary>
        /// Получить ближайший следующий день недели относительно dateFrom, при необходимости пропуская указанное
        /// количество недель.
        /// </summary>
        /// <param name="targetDay"></param>
        /// <param name="dateFrom"></param>
        /// <param name="weeksToSkip"></param>
        /// <returns></returns>
        public static DateOnly GetNext(this DayOfTheWeek targetDay, DateOnly dateFrom, int weeksToSkip = 0)
        {
            var daysUntilTarget = ((int)targetDay - (int)dateFrom.DayOfWeek.MapToDayOfTheWeek() + 7) % 7;

            var daysToAdd = daysUntilTarget == 0 ? 7 : daysUntilTarget;

            return dateFrom.AddDays(daysToAdd + weeksToSkip * 7);
        }
    }
}
