using Kventin.DataAccess.Enums;
using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.Marks
{
    public class MarkInfoForLessonDto
    {
        [Required]
        public required string MarkType { get; set; }

        [Required]
        public required int MarkId { get; set; }

        [Required]
        public required MarkValue MarkValue { get; set; }

        public string? TeacherComment { get; set; }
    }
}
