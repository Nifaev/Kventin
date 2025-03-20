using Kventin.DataAccess;
using Kventin.DataAccess.Domain;
using Kventin.Services.Dtos.Exercises;
using Kventin.Services.Dtos.Lessons;
using Kventin.Services.Infrastructure.Exceptions;
using Kventin.Services.Infrastructure.Extensions;
using Kventin.Services.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Kventin.Services.Services
{
    public class LessonService(KventinContext db) : ILessonService
    {
        private readonly KventinContext _db = db;

        public async Task<GetLessonInfoForStudentOrParentDto> GetStudentOrParentLessonInfo(int lessonId, int userId)
        {
            var lesson = await GetLessonFullInfo(lessonId);

            if (lesson == null)
                throw new EntityNotFoundException("Занятие с указанным id не найдено");

            var userChildren = await _db.Users
                .Where(x => x.Parents
                    .Select(y => y.Id)
                    .Contains(userId))
                .Select(x => x.Id)
                .ToListAsync();

            var isStudent = lesson.StudyGroup.Students.Any(x => x.Id == userId);

            var isParent = lesson.StudyGroup.Students.Any(x => userChildren.Contains(x.Id));

            if (!isStudent && !isParent)
                throw new NoAccessException("Вы можете просматривать только занятия ваших групп или групп ваших детей");

            User student;

            // Если детей несколько, то нужно получить того ребенка, который выбран
            // Надо реализовать
            // Может даже разбить на два метода: для родителей и для ребенка?
            if (isStudent)
                student = lesson.StudyGroup.Students.First(x => x.Id == userId);
            else if (isParent)
            {
                //student = lesson.StudyGroup.Students.First(x => )
            }

            var result = FillLessonDtoWithBaseInfo<GetLessonInfoForStudentOrParentDto>(lesson);

            throw new NotImplementedException();
        }

        public async Task<GetLessonInfoForTeacherDto> GetTeacherLessonInfo(int lessonId, int userId)
        {
            var lesson = await GetLessonFullInfo(lessonId);

            if (lesson == null)
                throw new EntityNotFoundException("Занятие с указанным id не найдено");

            var isSuperUser = (await _db.Users.FindAsync(userId))!.IsSuperUser;

            if (lesson.TeacherId != userId && !isSuperUser)
                throw new NoAccessException("Вы можете просматривать только те занятия, в которых назначены преподавателем");

            var result = FillLessonDtoWithBaseInfo<GetLessonInfoForTeacherDto>(lesson);

            result.Students = lesson.StudentsAttended
                .Select(x =>
                {
                    var mark = lesson.Marks.FirstOrDefault(y => y.StudentId == x.Id);

                    return new StudentLessonDto
                    {
                        StudentId = x.Id,
                        StudentFullName = x.GetFullName(),
                        StudentShortName = x.GetShortName(),
                        Attended = true,
                        Mark = mark?.Value,
                        TeacherComment = mark?.Comment,
                    };
                })
                .ToList();

            var notAttendedStudents = lesson.StudyGroup.Students
                .Where(x => !result.Students.Select(y => y.StudentId).Contains(x.Id))
                .Select(x => new StudentLessonDto
                {
                    StudentId = x.Id,
                    StudentFullName = x.GetFullName(),
                    StudentShortName = x.GetShortName(),
                    Attended = false,
                })
                .ToList();

            result.Students.AddRange(notAttendedStudents);

            result.Exercises = lesson.Exercises
                .Select(x => new TeacherExerciseShortInfoDto
                { 
                    ExerciseId = x.Id,
                    Deadline = x.DeadlineDateTime,
                    Content = x.Content,
                    IsIndividual = x.IsIndividual,
                    IndividualStudentId = x.IndividualStudentId,
                    IndividualStudentFullName = x.IndividualStudent?.GetFullName(),
                    IndividualStudentShortName = x.IndividualStudent?.GetShortName(),
                })
                .ToList();

            return result;
        }

        private async Task<Lesson?> GetLessonFullInfo(int lessonId)
        {
            var lesson = await _db.Lessons
                .Include(x => x.Subject)
                .Include(x => x.Teacher)
                .Include(x => x.Marks)
                .Include(x => x.StudentsAttended)
                .Include(x => x.StudyGroup)
                    .ThenInclude(x => x.Students)
                .Include(x => x.Exercises)
                .FirstOrDefaultAsync(x => x.Id == lessonId);

            return lesson;
        }

        private T FillLessonDtoWithBaseInfo<T>(Lesson lesson) where T : GetLessonBaseInfoDto
        {
            var baseDto = new GetLessonBaseInfoDto
            {
                LessonId = lesson.Id,
                SubjectId = lesson.SubjectId,
                SubjectName = lesson.Subject.Name,
                Date = lesson.Date,
                StartTime = lesson.StartTime,
                EndTime = lesson.EndTime,
                Classroom = lesson.Classroom,
                TeacherFullName = lesson.Teacher.GetFullName(),
                TeacherId = lesson.TeacherId,
                IsOnline = lesson.IsOnline,
                Topic = lesson.Topic,
                Description = lesson.Description,
                Status = lesson.Status,
                GroupName = lesson.StudyGroup.Name,
                GroupId = lesson.StudyGroup.Id,
            };

            var result = baseDto as T;

            return result!;
        }
    }
}
