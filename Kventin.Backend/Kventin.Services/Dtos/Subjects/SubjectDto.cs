using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.Subjects
{
    public class SubjectDto
    {
        [Required]
        public required long SubjectId { get; set; }

        [Required]
        public required string SubjectName { get; set; }
    }
}
