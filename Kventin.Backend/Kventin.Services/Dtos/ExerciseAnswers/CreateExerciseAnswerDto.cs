using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.ExerciseAnswers
{
    public class CreateExerciseAnswerDto
    {
        [Required]
        public required string Content { get; set; }

        [Required]
        public required long ExerciseId { get; set; }
    }
}
