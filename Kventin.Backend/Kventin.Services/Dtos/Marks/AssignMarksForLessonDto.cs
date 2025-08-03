using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.Marks
{
    public class AssignMarksForLessonDto
    {
        [Required]
        public int LessonId { get; set; }

        public List<StudentMarksDto> StudentMarks { get; set; } = [];
    }
}
