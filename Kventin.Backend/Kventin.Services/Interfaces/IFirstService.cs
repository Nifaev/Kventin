using Kventin.DataAccess.Domain;
using Kventin.Services.Dtos;

namespace Kventin.Services.Interfaces
{
    public interface IFirstService
    {
        Task<GetSubjectsDto> GetSubjects();
        Task CreateSubject(CreateSubjectDto subjectDto);
    }
}
