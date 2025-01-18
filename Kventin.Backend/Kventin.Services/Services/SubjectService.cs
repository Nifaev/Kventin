using Kventin.DataAccess;
using Kventin.DataAccess.Domain;
using Kventin.Services.Dtos;
using Kventin.Services.Infrastructure.Exceptions;
using Kventin.Services.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Kventin.Services.Services
{
    public class SubjectService(
        KventinContext db) : ISubjectService
    {
        private readonly KventinContext _db = db;

        public async Task CreateSubject(SubjectDto dto)
        {
            var subject = new Subject { Name = dto.Name };

            await _db.Subjects.AddAsync(subject);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteSubjectById(int id)
        {
            var subject = await _db.Subjects
                .FirstOrDefaultAsync(x => x.Id == id) 
                ?? throw new EntityNotFoundException("Предмет с таким id не найден");

            _db.Subjects.Remove(subject);
            await _db.SaveChangesAsync();
        }

        public async Task<List<SubjectDto>> GetAllSubjects()
        {
            var result = await _db.Subjects
                .Select(x => new SubjectDto { Name = x.Name })
                .ToListAsync();

            return result;
        }

        public async Task<SubjectDto> GetSubjectByid(int id)
        {
            var subject = await _db.Subjects
                .FirstOrDefaultAsync(x => x.Id == id) 
                ?? throw new EntityNotFoundException("Предмет с таким id не найден");

            return new SubjectDto { Name = subject.Name };
        }
    }
}
