using Kventin.DataAccess.Enums;

namespace Kventin.Services.Dtos.Lessons
{
    public class UpdateLessonDto
    {
        public long? TeacherId { get; set; }
        public long? SubjectId { get; set; }
        public string? Classroom { get; set; }
        public bool? IsOnline { get; set; }
        public string? Topic { get; set; }
        public string? Description { get; set; }
        public LessonStatus? LessonStatus { get; set; }

    }
}
