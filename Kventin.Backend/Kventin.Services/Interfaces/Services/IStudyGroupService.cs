using Kventin.Services.Dtos.StudyGroups;

namespace Kventin.Services.Interfaces.Services
{
    public interface IStudyGroupService
    {
        public Task CreateStudyGroup(CreateStudyGroupDto dto);
        public Task DeleteStudyGroup(long studyGroupId);
        public Task UpdateStudyGroup(long studyGroupId, UpdateStudyGroupDto dto);
        public Task<StudyGroupFullInfoDto> GetStudyGroupFullInfo(long studyGroupId);
        public Task<List<StudyGroupShortInfoDto>> GetAllStudyGroupsShortInfo(long userId, List<string> userRoles, long childId);
        public Task AddStudentToStudyGroup(long studyGroupId, long studentId);
        public Task DeleteStudentFromStudyGroup(long studyGroupId, long studentId);
    }
}
