using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Kventin.Services.Dtos.StudyGroups
{
    public class CreateStudyGroupDto
    {
        [Required]
        public required string GroupName { get; set; }

        [Required]
        public required long SubjectId { get; set; }

        [Required]
        public required long TeacherId { get; set; }
    }
}
