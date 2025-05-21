using Kventin.DataAccess;
using Kventin.DataAccess.Domain;
using Kventin.DataAccess.Enums;
using Kventin.Services.Config;
using Kventin.Services.Dtos.Exercises;
using Kventin.Services.Dtos.Files;
using Kventin.Services.Dtos.Lessons;
using Kventin.Services.Dtos.Marks;
using Kventin.Services.Dtos.Users;
using Kventin.Services.Infrastructure.Exceptions;
using Kventin.Services.Infrastructure.Extensions;
using Kventin.Services.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Kventin.Services.Services
{
    public class LessonService(KventinContext db,
        IOptions<LessonsGeneratorOptions> lessonsOptions,
        IFileService fileService) : ILessonService
    {
        private readonly KventinContext _db = db;
        private readonly IFileService _fileService = fileService;
        private readonly LessonsGeneratorOptions _options = lessonsOptions.Value;
        private readonly int _take = 100;
        public async Task<SchoolWeekDto> GetSchoolWeek(int skipWeeksCount, int userId, List<string> userRoles, int childId)
        {
            if (skipWeeksCount > _options.WeeksCount)
                throw new ArgumentException($"Значение параметра skipWeeksCount не должно превышать {_options.WeeksCount}");

            var (startOfWeek, endOfWeek) = GetWeek(skipWeeksCount);

            var result = new SchoolWeekDto
                {
                    SchoolDays = [],
                    StartOfWeek = startOfWeek,
                    EndOFWeek = endOfWeek
                };

            var lessonsQuery = _db.Lessons
                .Where(x => x.Date >= startOfWeek &&
                            x.Date <= endOfWeek);

            lessonsQuery = GetLessonsQueryForRoles(lessonsQuery, userId, userRoles, childId);

            lessonsQuery = lessonsQuery
                .Include(x => x.Teacher)
                .Include(x => x.Subject)
                .Include(x => x.StudyGroup);

            var lessons = await GetEntitiesPaged(lessonsQuery);

            if (!lessons.Any())
                return result;

            result.SchoolDays = lessons
                .GroupBy(x => x.Date)
                .ToDictionary(
                    x => x.Key,
                    y => y
                        .OrderBy(y => y.StartTime)
                            .ThenBy(y => y.EndTime)
                        .ToList())
                .Select(x => new SchoolDayDto
                {
                    Date = x.Key,
                    DayOfWeek = x.Key.DayOfWeek.MapToDayOfTheWeek(),
                    Lessons = x.Value
                        .Select(y => new LessonShortInfoDto(y))
                        .ToList()
                })
                .ToList();

            return result;
        }

        public async Task<SchoolDayDto> GetSchoolDay(DateOnly date, int userId, List<string> userRoles, int childId)
        {
            var result = new SchoolDayDto
            {
                Date = date,
                DayOfWeek = date.DayOfWeek.MapToDayOfTheWeek(),
                Lessons = []
            };

            var lessonsQuery = _db.Lessons
                .Where(x => x.Date == date);

            lessonsQuery = GetLessonsQueryForRoles(lessonsQuery, userId, userRoles, childId);

            lessonsQuery = lessonsQuery
                .Include(x => x.Teacher)
                .Include(x => x.StudyGroup)
                .Include(x => x.Subject);

            var lessons = await GetEntitiesPaged(lessonsQuery);

            if (!lessons.Any())
                return result;

            result.Lessons = lessons
                .OrderBy(x => x.StartTime)
                    .ThenBy(x => x.EndTime)
                .Select(x => new LessonShortInfoDto(x))
                .ToList();

            return result;
        }

        public async Task<LessonFullInfoDto> GetLessonFullInfo(int lessonId, int userId, List<string> userRoles, int childId)
        {
            var studentId = 0;

            if (userRoles.Contains("Parent"))
            {
                if (childId == 0)
                    throw new ArgumentException("Не выбран ребенок");

                studentId = childId;
            }
            else if (userRoles.Contains("Student"))
                studentId = userId;

            var lesson = await GetLessonWithAllIncludes(lessonId);

            if (lesson == null)
                throw new EntityNotFoundException("Занятие с таким Id не найдено");

            var dto = GetLessonFullDtoWithBaseInfo(lesson);

            if (lesson.Files.Count != 0)
            {
                var fileDtos = lesson.Files
                    .Select(x => new FileInfoDto(x))
                    .ToList();

                dto.Files.AddRange(fileDtos);
            }

            var studentsEnumerable = lesson.StudyGroup.Students
                .Select(x => new LessonStudentInfoDto(x)
                {
                    Attended = lesson.StudentsAttended
                        .Any(y => y.Id == x.Id),
                    Marks = lesson.Marks
                        .Where(x => x.StudentId == x.Id &&
                                    x.LessonId.HasValue &&
                                    x.MarkType == MarkType.ForLesson)
                        .Select(x => new MarkInfoDto
                        {
                            MarkType = x.MarkType.GetDescription(),
                            MarkId = x.Id,
                            MarkValue = x.Value,
                            TeacherComment = x.Comment,
                        })
                        .ToList(),
                });

            var exercisesEnumerable = lesson.Exercises
                .Select(x => new ExerciseShortInfoDto
                {
                    ExeriseId = x.Id,
                    IsIndividual = x.IsIndividual,
                    IndividualStudent = x.IsIndividual && x.IndividualStudentId.HasValue
                        ? new UserShortInfoDto(x.IndividualStudent!)
                        : null,
                    CreateDateTime = x.CreateDateTime,
                    DeadlineDateTime = x.DeadlineDateTime,
                });

            if (studentId != 0)
            {
                var student = studentsEnumerable
                    .FirstOrDefault(x => x.UserId == studentId);

                if (student == null)
                    throw new ArgumentException("Указанный ученик не найден среди учеников группы");

                dto.Students.Add(student);

                if (lesson.Exercises.Count != 0)
                {
                    var exercises = exercisesEnumerable
                        .Where(x => x.IsIndividual &&
                                    x.IndividualStudent != null &&
                                    x.IndividualStudent!.UserId == studentId ||
                                    !x.IsIndividual)
                        .ToList();

                    exercises.ForEach(x =>
                    {
                        var mark = lesson.Exercises
                            .FirstOrDefault(y => y.Id == x.ExeriseId)?
                            .Marks
                            .FirstOrDefault(y => y.StudentId == studentId);

                        if (mark == null)
                        {
                            x.Mark = null;

                            return;
                        }

                        var markDto = new MarkInfoDto
                        {
                            MarkId = mark.Id,
                            MarkValue = mark.Value,
                            MarkType = mark.MarkType.GetDescription(),
                            TeacherComment = mark.Comment,
                        };

                        x.Mark = markDto;
                    });

                    dto.Exercises = exercises;
                }
            }
            else
            {
                var students = studentsEnumerable.ToList();
                var exercises = exercisesEnumerable.ToList();

                dto.Students = students;
                dto.Exercises = exercises;
            }

            return dto;
        }

        private LessonFullInfoDto GetLessonFullDtoWithBaseInfo(Lesson lesson)
        {
            var dto = new LessonFullInfoDto
            {
                LessonId = lesson.Id,
                SubjectId = lesson.SubjectId,
                SubjectName = lesson.Subject.Name,
                Date = lesson.Date,
                StartTime = lesson.StartTime,
                EndTime = lesson.EndTime,
                Classroom = lesson.Classroom,
                Teacher = new UserShortInfoDto(lesson.Teacher),
                IsOnline = lesson.IsOnline,
                Topic = lesson.Topic,
                Description = lesson.Description,
                LessonStatus = lesson.Status.GetDescription(),
                GroupName = lesson.StudyGroup.Name,
                GroupId = lesson.StudyGroup.Id,
                Students = [],
                Files = [],
                Exercises = [],
            };

            return dto;
        }

        private async Task<List<T>> GetEntitiesPaged<T>(IQueryable<T> query)
        {
            var entitiesCount = await query.CountAsync();
            var pageCount = entitiesCount / (double)_take;
            var entities = new List<T>();

            if (entitiesCount == 0)
                return entities;

            for (int pageNumber = 0; pageNumber < pageCount; pageNumber++)
            {
                var partOfEntities = await query
                    .Skip(pageNumber * _take)
                    .Take(_take)
                    .ToListAsync();

                entities.AddRange(partOfEntities);
            }

            return entities;
        }
        
        private async Task<Lesson?> GetLessonWithAllIncludes(int lessonId)
        {
            var lesson = await _db.Lessons
                .AsNoTracking()
                .Include(x => x.Subject)
                .Include(x => x.Teacher)
                .Include(x => x.Marks)
                .Include(x => x.StudentsAttended)
                .Include(x => x.StudyGroup)
                    .ThenInclude(x => x.Students)
                .Include(x => x.Exercises)
                    .ThenInclude(x => x.IndividualStudent)
                .Include(x => x.Files)
                    .ThenInclude(x => x.UploadedByUser)
                .FirstOrDefaultAsync(x => x.Id == lessonId);

            return lesson;
        }

        private IQueryable<Lesson> GetLessonsQueryForRoles(IQueryable<Lesson> lessonsQuery, int userId, List<string> userRoles, int childId)
        {
            IQueryable<Lesson> result = lessonsQuery;

            if (userRoles.Contains("Parent"))
            {
                if (childId == 0)
                    throw new ArgumentException("Вы не выбрали ребенка");
                else
                    result = result
                        .Where(x => x.StudyGroup.Students.
                            Any(y => y.Id == childId));
            }
            else if (userRoles.Contains("Teacher"))
                result = result
                    .Where(x => x.TeacherId == userId);
            else if (userRoles.Contains("Student"))
                result = result
                    .Where(x => x.StudyGroup.Students
                        .Any(y => y.Id == userId));

            return result;
        }

        private (DateOnly StartOfWeek, DateOnly EndOfWeek) GetWeek(int skipWeeksCount)
        {
            var today = DateTime.Today.AddDays(7 * skipWeeksCount);

            var diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;

            var startOfWeek = DateOnly.FromDateTime(today.AddDays(-1 * diff).Date);

            var endOfWeek = startOfWeek.AddDays(6);

            return (startOfWeek, endOfWeek);
        }

        public async Task UpdateLesson(int lessonId, UpdateLessonDto dto)
        {
            var lesson = await _db.Lessons.FindAsync(lessonId)
                ?? throw new EntityNotFoundException("Занятие с указанным Id не найдено");

            if (dto.IsOnline.HasValue)
                lesson.IsOnline = dto.IsOnline.Value;

            if (dto.LessonStatus.HasValue)
                dto.LessonStatus = dto.LessonStatus.Value;

            if (!string.IsNullOrWhiteSpace(dto.Classroom))
                lesson.Classroom = dto.Classroom;

            if (!string.IsNullOrWhiteSpace(dto.Description))
                lesson.Description = dto.Description;

            if (!string.IsNullOrWhiteSpace(dto.Topic))
                lesson.Topic = dto.Topic;

            if (dto.TeacherId.HasValue)
            {
                var teacher = await _db.Users
                    .FirstOrDefaultAsync(x => x.Id == dto.TeacherId.Value &&
                                              x.Roles.Any(y => y.Name == "Teacher"))
                    ?? throw new EntityNotFoundException("Преподаватель с таким Id не найден");

                lesson.TeacherId = teacher.Id;
                lesson.Teacher = teacher;
            }

            if (dto.SubjectId.HasValue)
            {
                var subject = await _db.Subjects.FindAsync(dto.SubjectId.Value)
                    ?? throw new EntityNotFoundException("Предмет с таким Id не найден");

                lesson.SubjectId = subject.Id;
                lesson.Subject = subject;
            }

            await _db.SaveChangesAsync();
        }

        public async Task AttachFilesToLesson(int lessonId, List<IFormFile> files, int uploadedByUserId)
        {
            await _fileService.UploadFiles<Lesson>(files, uploadedByUserId, FileLinkType.Lesson, lessonId);
        }

        public async Task DetachFilesFromLesson(int lessonId, List<int> fileIds)
        {
            await _fileService.DeleteFiles(fileIds);
        }

        public async Task MarkAttendance(int lessonId, List<int> studentIds)
        {
            var students = await _db.Users
                .Where(x => studentIds.Contains(x.Id) &&
                            x.Roles.Any(y => y.Name == "Student"))
                .ToListAsync();

            if (students.Count == 0)
                throw new EntityNotFoundException("Ни один ученик с переданным Id не найден");

            var lesson = await _db.Lessons.FindAsync(lessonId)
                ?? throw new EntityNotFoundException("Занятие с переданным Id не найдено");

            lesson.StudentsAttended.AddRange(students);

            await _db.SaveChangesAsync();
        }
    }
}
