using Kventin.DataAccess.Enums;

namespace Kventin.Services.Dtos.Lessons
{
    public class StudentLessonDto
    {
        public int StudentId { get; set; }
        public string? StudentFullName { get; set; }
        public string? StudentShortName { get; set; }
        public bool Attended { get; set; }
        public MarkValue? Mark {  get; set; } 
        public string? TeacherComment { get; set; }
    }
}
