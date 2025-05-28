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
        public long StudyGroupId { get; set; }

        [Required]
        public long LessonId { get; set; }

        public long? IndividualStudentId { get; set; }
    }
}
