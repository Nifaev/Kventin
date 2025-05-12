using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.StudyGroups
{
    public class CreateStudyGroupDto
    {
        [Required]
        public required string GroupName { get; set; }

        [Required]
        public required int SubjectId { get; set; }

        [Required]
        public required int TeacherId { get; set; }
    }
}
