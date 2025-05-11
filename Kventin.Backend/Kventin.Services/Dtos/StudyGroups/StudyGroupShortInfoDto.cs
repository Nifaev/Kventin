using Kventin.Services.Dtos.Users;

namespace Kventin.Services.Dtos.StudyGroups
{
    public class StudyGroupShortInfoDto
    {
        public required int StudyGroupId { get; set; }
        public required UserShortInfoDto Teacher { get; set; }
        public required string SubjectName { get; set; }
        public required string GroupName { get; set; }
    }
}
