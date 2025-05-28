using Kventin.DataAccess.Enums;

namespace Kventin.Services.Dtos.Schedule
{
    public class UpdateScheduleItemDto
    {
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public DayOfTheWeek? DayOfWeek { get; set; }
        public bool? IsOnline { get; set; }
        public string? Classroom { get; set; }
        public long? SubjectId { get; set; }
        public long? StudyGroupId { get; set; }
        public long? TeacherId { get; set; }
    }
}
