using Kventin.Services.Dtos.Users;
using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.StudyGroups
{
    public class StudyGroupFullInfoDto
    {
        [Required]
        public required int StudyGroupId { get; set; }

        [Required]
        public required string GroupName { get; set; }

        [Required]
        public required string SubjectName { get; set; }

        [Required]
        public required int SubjectId { get; set; }

        [Required]
        public required UserShortInfoDto Teacher { get; set; }

        public List<UserShortInfoDto> Students { get; set; } = [];
    }
}
