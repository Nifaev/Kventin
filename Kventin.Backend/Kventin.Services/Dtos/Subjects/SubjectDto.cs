using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.Subjects
{
    public class SubjectDto
    {
        [Required]
        public required int SubjectId { get; set; }

        [Required]
        public required string SubjectName { get; set; }
    }
}
