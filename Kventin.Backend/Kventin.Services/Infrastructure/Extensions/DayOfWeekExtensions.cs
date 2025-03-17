using Kventin.DataAccess.Enums;

namespace Kventin.Services.Infrastructure.Extensions
{
    public static class DayOfWeekExtensions
    {
        public static DayOfTheWeek MapToDayOfTheWeek(this DayOfWeek dayOfWeek)
        {
            var dayNumber = (int)dayOfWeek;

            dayNumber = dayNumber == 0 ? dayNumber + 1 : dayNumber;

            return (DayOfTheWeek)dayNumber;
        }
    }
}
