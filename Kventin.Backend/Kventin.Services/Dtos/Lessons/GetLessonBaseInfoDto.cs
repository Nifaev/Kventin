using Kventin.DataAccess.Enums;

namespace Kventin.Services.Dtos.Lessons
{
    public class GetLessonBaseInfoDto
    {
        public required int SubjectId { get; set; }
        public required string SubjectName { get; set; }
        public required int LessonId { get; set; }
        public required DateOnly Date { get; set; }
        public required TimeOnly StartTime { get; set; }
        public required TimeOnly EndTime { get; set; }
        public string? Classroom { get; set; }
        public string? TeacherFullName { get; set; }
        public required int TeacherId { get; set; }
        public required bool IsOnline {  get; set; }
        public string? Topic { get; set; }
        public string? Description { get; set; }
        public required LessonStatus Status { get; set; }
        public required string GroupName { get; set; }
        public required int GroupId { get; set; }
    }
}
