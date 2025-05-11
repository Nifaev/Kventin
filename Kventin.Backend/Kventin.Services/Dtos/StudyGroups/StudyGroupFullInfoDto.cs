using Kventin.Services.Dtos.Users;

namespace Kventin.Services.Dtos.StudyGroups
{
    public class StudyGroupFullInfoDto
    {
        public required int StudyGroupId { get; set; }
        public required string GroupName { get; set; }
        public required string SubjectName { get; set; }
        public required int SubjectId { get; set; }
        public required UserShortInfoDto Teacher { get; set; }
        public List<UserShortInfoDto> Students { get; set; } = [];
    }
}
