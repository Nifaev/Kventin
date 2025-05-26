using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.Exercises
{
    public class CreateExerciseDto
    {
        public DateTime? DeadlineDateTime { get; set; }

        [Required]
        public required string Content { get; set; }

        [Required]
        public required bool IsIndividual { get; set; }

        [Required]
        public int StudyGroupId { get; set; }

        [Required]
        public int LessonId { get; set; }

        public int? IndividualStudentId { get; set; }
    }
}
