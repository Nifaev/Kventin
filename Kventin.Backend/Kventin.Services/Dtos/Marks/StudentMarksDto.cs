using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.Marks
{
    public class StudentMarksDto
    {
        [Required]
        public long StudentId { get; set; }

        public List<MarkShortInfoDto> Marks { get; set; } = [];
    }
}
