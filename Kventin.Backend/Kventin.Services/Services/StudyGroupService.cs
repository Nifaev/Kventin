using Kventin.DataAccess;
using Kventin.DataAccess.Domain;
using Kventin.Services.Dtos.StudyGroups;
using Kventin.Services.Dtos.Users;
using Kventin.Services.Infrastructure.Exceptions;
using Kventin.Services.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Kventin.Services.Services
{
    public class StudyGroupService(KventinContext db) : IStudyGroupService
    {
        private readonly KventinContext _db = db;

        public async Task AddStudentToStudyGroup(long studyGroupId, long studentId)
        {
            var studyGroup = await _db.StudyGroups.FindAsync(studyGroupId)
                ?? throw new EntityNotFoundException("Группа с таким Id не найдена");

            var student = await _db.Users
                .FirstOrDefaultAsync(x => x.Id == studentId &&
                                          x.Roles.Any(x => x.Name == "Student"))
                ?? throw new EntityNotFoundException("Ученик с таким Id не найден");

            studyGroup.Students.Add(student);

            await _db.SaveChangesAsync();
        }

        public async Task DeleteStudentFromStudyGroup(long studyGroupId, long studentId)
        {
            var studyGroup = await _db.StudyGroups
                .Include(x => x.Students)
                .FirstOrDefaultAsync(x => x.Id == studyGroupId)
                ?? throw new EntityNotFoundException("Группа с таким Id не найдена");

            var student = await _db.Users
                .FirstOrDefaultAsync(x => x.Id == studentId &&
                                          x.Roles.Any(x => x.Name == "Student"))
                ?? throw new EntityNotFoundException("Ученик с таким Id не найден");

            studyGroup.Students.Remove(student);

            await _db.SaveChangesAsync();
        }

        public async Task CreateStudyGroup(CreateStudyGroupDto dto)
        {
            var subject = await _db.Subjects.FindAsync(dto.SubjectId);

            if (subject == null)
                throw new EntityNotFoundException("Предмет с таким Id не найден");

            if (string.IsNullOrWhiteSpace(dto.GroupName))
                throw new ArgumentException("Название группы должно содержать хотя бы 1 символ");

            var teacher = await _db.Users
                .FirstOrDefaultAsync(x => x.Id == dto.TeacherId &&
                                          x.Roles.Any(y => y.Name == "Teacher")) 
                ?? throw new EntityNotFoundException("Преподаватель с таким Id не найден");
            
            var studyGroup = new StudyGroup
            {
                TeacherId = dto.TeacherId,
                SubjectId = dto.SubjectId,
                Teacher = teacher,
                Name = dto.GroupName,
                Subject = subject,
            };

            await _db.StudyGroups.AddAsync(studyGroup);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteStudyGroup(long studyGroupId)
        {
            var group = await _db.StudyGroups.FindAsync(studyGroupId);

            if (group != null)
            {
                _db.StudyGroups.Remove(group);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<List<StudyGroupShortInfoDto>> GetAllStudyGroupsShortInfo(long userId, List<string> userRoles, long childId)
        {
            var result = new List<StudyGroupShortInfoDto>();

            var groupsQuery = _db.StudyGroups.AsQueryable();

            if (userRoles.Contains("Parent"))
            {
                if (childId == 0)
                    throw new Exception("Не выбран ребенок");
                else
                    groupsQuery = groupsQuery.Where(x => x.Students.Any(y => y.Id == childId));
            }
            else if (userRoles.Contains("Student"))
                groupsQuery = groupsQuery.Where(x => x.Students.Any(y => y.Id == userId));
            else if (userRoles.Contains("Teacher"))
                groupsQuery = groupsQuery.Where(x => x.TeacherId == userId);

            groupsQuery = groupsQuery
                .Include(x => x.Teacher)
                .Include(x => x.Subject);

            var take = 100;
            var groupsCount = await groupsQuery.CountAsync();
            var pageCount = Math.Ceiling(groupsCount / (double)take);

            for (int pageNumber = 0; pageNumber < pageCount; pageNumber++)
            {
                var groups = await groupsQuery
                    .Skip(pageNumber *  take)
                    .Take(take)
                    .ToListAsync();

                var dtos = groups.Select(x => new StudyGroupShortInfoDto
                    {
                        Teacher = new UserShortInfoDto(x.Teacher),
                        SubjectName = x.Subject.Name,
                        StudyGroupId = x.Id,
                        GroupName = x.Name,
                    })
                    .ToList();

                result.AddRange(dtos);
            }

            return result;
        }

        public async Task<StudyGroupFullInfoDto> GetStudyGroupFullInfo(long studyGroupId)
        {
            var studyGroup = await _db.StudyGroups
                .Include(x => x.Teacher)
                .Include(x => x.Subject)
                .Include(x => x.Students)
                .FirstOrDefaultAsync(x => x.Id == studyGroupId) 
                ?? throw new EntityNotFoundException("Занятие с таким Id не найдено");
            
            var result = new StudyGroupFullInfoDto
            {
                GroupName = studyGroup.Name,
                StudyGroupId = studyGroupId,
                Teacher = new UserShortInfoDto(studyGroup.Teacher),
                SubjectId = studyGroup.SubjectId,
                SubjectName = studyGroup.Subject.Name,
                Students = studyGroup.Students
                    .Select(x => new UserShortInfoDto(x))
                    .ToList(),
            };

            return result;
        }

        public async Task UpdateStudyGroup(long studyGroupId, UpdateStudyGroupDto dto)
        {
            var group = await _db.StudyGroups.FindAsync(studyGroupId) 
                ?? throw new EntityNotFoundException("Группа с таким Id не найдена");

            if (!string.IsNullOrWhiteSpace(dto.GroupName))
                group.Name = dto.GroupName;

            if (dto.SubjectId.HasValue)
            {
                var subject = await _db.Subjects.FindAsync(dto.SubjectId.Value) 
                    ?? throw new EntityNotFoundException("Предмет с таким Id не найден");
                
                group.Subject = subject;
            }

            if (dto.TeacherId.HasValue)
            {
                var teacher = await _db.Users
                    .FirstOrDefaultAsync(x => x.Id == dto.TeacherId.Value &&
                                              x.Roles.Any(y => y.Name == "Teacher")) 
                    ?? throw new EntityNotFoundException("Преподаватель с таким Id не найден");
                
                group.Teacher = teacher;
            }

            await _db.SaveChangesAsync();
        }
    }
}
