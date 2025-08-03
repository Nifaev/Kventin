using Kventin.Services.Dtos.Users;
using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.StudyGroups
{
    public class StudyGroupShortInfoDto
    {
        [Required]
        public required int StudyGroupId { get; set; }

        [Required]
        public required UserShortInfoDto Teacher { get; set; }

        [Required]
        public required string SubjectName { get; set; }

        [Required]
        public required string GroupName { get; set; }
    }
}
