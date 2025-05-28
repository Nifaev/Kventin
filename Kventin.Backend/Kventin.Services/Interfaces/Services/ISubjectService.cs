using Kventin.Services.Dtos.Subjects;

namespace Kventin.Services.Interfaces.Services
{
    public interface ISubjectService
    {
        public Task CreateSubject(string subjectName);
        public Task<List<SubjectDto>> GetAllSubjects(long userId, List<string> userRoles, long childId);
        public Task<SubjectDto> GetSubjectByid(long id);
        public Task DeleteSubjectById(long id);
        public Task UpdateSubjectById(long id, string newSubjectName);
    }
}
