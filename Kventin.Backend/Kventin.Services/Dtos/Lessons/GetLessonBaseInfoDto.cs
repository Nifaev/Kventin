using Kventin.DataAccess.Enums;

namespace Kventin.Services.Dtos.Lessons
{
    public class GetLessonBaseInfoDto
    {
        public int SubjectId { get; set; }
        public required string SubjectName { get; set; }
        public int LessonId { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string? Classroom { get; set; }
        public string? TeacherFullName { get; set; }
        public int TeacherId { get; set; }
        public bool IsOnline {  get; set; }
        public string? Topic { get; set; }
        public string? Description { get; set; }
        public LessonStatus Status { get; set; }
        public required string GroupName { get; set; }
        public int GroupId { get; set; }
    }
}
