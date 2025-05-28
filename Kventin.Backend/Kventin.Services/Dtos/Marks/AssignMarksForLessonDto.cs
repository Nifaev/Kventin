using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.Marks
{
    public class AssignMarksForLessonDto
    {
        [Required]
        public long LessonId { get; set; }

        public List<StudentMarksDto> StudentMarks { get; set; } = [];
    }
}
