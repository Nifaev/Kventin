using Kventin.DataAccess;
using Kventin.DataAccess.Domain;
using Kventin.Services.Dtos.Subjects;
using Kventin.Services.Infrastructure.Exceptions;
using Kventin.Services.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Kventin.Services.Services
{
    public class SubjectService(
        KventinContext db) : ISubjectService
    {
        private readonly KventinContext _db = db;

        public async Task CreateSubject(string subjectName)
        {
            var subject = new Subject { Name = subjectName };

            await _db.Subjects.AddAsync(subject);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteSubjectById(int subjectId)
        {
            var subject = await _db.Subjects
                .FindAsync(subjectId)
                ?? throw new EntityNotFoundException("Предмет с таким id не найден");

            _db.Subjects.Remove(subject);

            await _db.SaveChangesAsync();
        }

        public async Task<List<SubjectDto>> GetAllSubjects()
        {
            var result = await _db.Subjects
                .Select(x => new SubjectDto 
                { 
                    SubjectName = x.Name,
                    SubjectId = x.Id
                })
                .OrderBy(x => x.SubjectName)
                .ToListAsync();

            return result;
        }

        public async Task<SubjectDto> GetSubjectByid(int subjectId)
        {
            var subject = await _db.Subjects
                .FindAsync(subjectId)
                ?? throw new EntityNotFoundException("Предмет с таким id не найден");

            return new SubjectDto 
            { 
                SubjectName = subject.Name,
                SubjectId= subject.Id
            };
        }

        public async Task UpdateSubjectById(int subjectId, string newSubjectName)
        {
            var subject = await _db.Subjects
                .FindAsync(subjectId)
                ?? throw new EntityNotFoundException("Предмет с таким id не найден");

            if (!string.IsNullOrWhiteSpace(newSubjectName))
                subject.Name = newSubjectName;

            await _db.SaveChangesAsync();
        }
    }
}
