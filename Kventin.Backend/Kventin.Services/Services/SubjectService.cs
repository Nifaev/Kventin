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
        private readonly int _take = 100;

        public async Task CreateSubject(string subjectName)
        {
            if (string.IsNullOrWhiteSpace(subjectName))
                throw new ArgumentException("Нельзя создать предмет с пустым названием");

            var wasSubjectCreated = await _db.Subjects.AnyAsync(x => x.Name == subjectName);

            if (wasSubjectCreated)
                throw new EntityAlreadyCreatedException("Предмет с таким названием уже существует");

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

        public async Task<List<SubjectDto>> GetAllSubjects(int userId, List<string> userRoles, int childId)
        {
            var subjectsQuery = _db.Subjects.AsQueryable();

            if (userRoles.Contains("Parent"))
            {
                if (childId == 0)
                    throw new ArgumentException("Не выбран ребенок");

                subjectsQuery = subjectsQuery
                    .Where(x => x.StudyGroups
                        .Any(y => y.Students
                            .Any(z => x.Id == childId)));
            }
            else if (userRoles.Contains("Student"))
                subjectsQuery = subjectsQuery
                    .Where(x => x.StudyGroups
                        .Any(y => y.Students
                            .Any(y => y.Id == userId)));
            else if (userRoles.Contains("Teacher"))
                subjectsQuery = subjectsQuery
                    .Where(x => x.StudyGroups
                        .Any(y => y.TeacherId == userId));

            var result = new List<SubjectDto>();
            var subjectsCount = await subjectsQuery.CountAsync();
            var pageCount = Math.Ceiling(subjectsCount / (double)_take);

            if (subjectsCount == 0)
                return result;

            for (int pageNumber = 0;  pageNumber < pageCount; pageNumber++)
            {
                var dtos = await subjectsQuery
                    .Select(x => new SubjectDto
                        {
                            SubjectName = x.Name,
                            SubjectId = x.Id
                        })
                    .OrderBy(x => x.SubjectName)
                    .ToListAsync();

                result.AddRange(dtos);
            }

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
