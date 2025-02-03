using Kventin.DataAccess.Domain;
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
        public int? SubjectId { get; set; }
        public int? StudyGroupId { get; set; }
        public int? TeacherId { get; set; }
    }
}
