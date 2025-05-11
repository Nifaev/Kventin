using Kventin.DataAccess.Enums;

namespace Kventin.Services.Dtos.Schedule
{
    public class AddScheduleItemDto
    {
        public required DayOfTheWeek DayOfWeek { get; set; }
        public required TimeOnly StartTime { get; set; }
        public required TimeOnly EndTime { get; set; }
        public string? Classroom { get; set; }
        public required int TeacherId { get; set; }
        public required int GroupId { get; set; }
        public required int SubjectId { get; set; }
        public required int ScheduleId { get; set; }
        public required bool IsOnline { get; set; }
    }
}
