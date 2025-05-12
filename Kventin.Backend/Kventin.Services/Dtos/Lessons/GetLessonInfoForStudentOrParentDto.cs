using Kventin.Services.Dtos.Exercises;

namespace Kventin.Services.Dtos.Lessons
{
    public class GetLessonInfoForStudentOrParentDto : GetLessonBaseInfoDto
    {
        public List<StudentExerciseShortInfoDto> Exercises { get; set; } = [];
        public StudentLessonDto? StudentLessonInfo { get; set; }
        public required bool Attended { get; set; }
    }
}
