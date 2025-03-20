using Kventin.DataAccess.Enums;

namespace Kventin.Services.Dtos.Exercises
{
    public class StudentExerciseShortInfoDto : TeacherExerciseShortInfoDto
    {
        public MarkValue? MarkValue { get; set; }
    }
}
