using Kventin.Services.Dtos.Exercises;

namespace Kventin.Services.Dtos.Lessons
{
    public class GetLessonInfoForTeacherDto : GetLessonBaseInfoDto
    {
        public List<StudentLessonDto> Students { get; set; } = [];
        public List<TeacherExerciseShortInfoDto> Exercises { get; set; } = [];
    }
}
