using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.Marks
{
    public class AssignMarksForExerciseDto
    {
        [Required]
        public required int ExerciseId { get; set; }

        public List<StudentMarksDto> StudentMarks { get; set; } = [];
    }
}
