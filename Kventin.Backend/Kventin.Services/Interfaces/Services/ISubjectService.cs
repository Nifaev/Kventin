using Kventin.Services.Dtos.Subjects;

namespace Kventin.Services.Interfaces.Services
{
    public interface ISubjectService
    {
        public Task CreateSubject(string subjectName);
        public Task<List<SubjectDto>> GetAllSubjects(int userId, List<string> userRoles, int childId);
        public Task<SubjectDto> GetSubjectByid(int id);
        public Task DeleteSubjectById(int id);
        public Task UpdateSubjectById(int id, string newSubjectName);
    }
}
