using Kventin.DataAccess.Enums;

namespace Kventin.Services.Dtos.Schedule
{
    public class AddScheduleItemDto
    {
        public DayOfTheWeek DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string? Classroom { get; set; }
        public int TeacherId { get; set; }
        public int GroupId { get; set; }
        public int SubjectId { get; set; }
        public int ScheduleId { get; set; }
        public bool IsOnline { get; set; }
    }
}
