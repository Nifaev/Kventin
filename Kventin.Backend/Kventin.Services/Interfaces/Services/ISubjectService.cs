using Kventin.Services.Dtos;

namespace Kventin.Services.Interfaces.Services
{
    public interface ISubjectService
    {
        public Task CreateSubject(SubjectDto dto);
        public Task<List<SubjectDto>> GetAllSubjects();
        public Task<SubjectDto> GetSubjectByid(int id);
        public Task DeleteSubjectById(int id);
    }
}
