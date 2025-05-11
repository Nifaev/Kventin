using Kventin.Services.Dtos.StudyGroups;

namespace Kventin.Services.Interfaces.Services
{
    public interface IStudyGroupService
    {
        public Task CreateStudyGroup(CreateStudyGroupDto dto);
        public Task DeleteStudyGroup(int studyGroupId);
        public Task UpdateStudyGroup(int studyGroupId, UpdateStudyGroupDto dto);
        public Task<StudyGroupFullInfoDto> GetStudyGroupFullInfo(int studyGroupId);
        public Task<List<StudyGroupShortInfoDto>> GetAllStudyGroupsShortInfo();
        public Task AddStudentToStudyGroup(int studyGroupId, int studentId);
        public Task DeleteStudentFromStudyGroup(int studyGroupId, int studentId);
    }
}
