namespace Kventin.Services.Dtos.Schedule
{
    public class ReturnScheduleItemDto
    {
        public int ScheduleItemId { get; set; }
        public required string DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string? Classroom { get; set; }
        public string? TeacherShortName { get; set; }
        public required string GroupName { get; set; }
        public required string SubjectName { get; set; }
        public bool IsOnline { get; set; }
    }
}
