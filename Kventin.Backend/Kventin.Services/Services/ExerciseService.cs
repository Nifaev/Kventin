using Kventin.DataAccess;
using Kventin.DataAccess.Domain;
using Kventin.DataAccess.Enums;
using Kventin.Services.Dtos.ExerciseAnswers;
using Kventin.Services.Dtos.Exercises;
using Kventin.Services.Dtos.Files;
using Kventin.Services.Dtos.Marks;
using Kventin.Services.Dtos.Users;
using Kventin.Services.Infrastructure.Exceptions;
using Kventin.Services.Infrastructure.Extensions;
using Kventin.Services.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Kventin.Services.Services
{
    public class ExerciseService(KventinContext db,
        IFileService fileService) : IExerciseService
    {
        private readonly KventinContext _db = db;
        private readonly IFileService _fileService = fileService;

        public async Task CreateExercise(long teacherId, CreateExerciseDto dto)
        {
            if (dto.IsIndividual && !dto.IndividualStudentId.HasValue)
                throw new ArgumentException("Индивидуальное занятие должно быть кому то назначено");

            var teacher = await _db.Users.FindAsync(teacherId);

            var lesson = await _db.Lessons
                .Include(x => x.Teacher)
                .FirstOrDefaultAsync(x => x.Id == dto.LessonId)
                ?? throw new EntityNotFoundException("Занятие с таким Id не найдено");

            if (lesson.TeacherId != teacherId)
                throw new NoAccessException("Вы не моежете выдавать задания, т.к. не вы ведете данное занятие");

            var studyGroup = await _db.StudyGroups.FindAsync(dto.StudyGroupId)
                ?? throw new EntityNotFoundException("Группа с таким Id не найдена");

            User? individualStudent = null;

            if (dto.IsIndividual && dto.IndividualStudentId.HasValue)
                individualStudent = await _db.Users
                    .FirstOrDefaultAsync(x => x.Id == dto.IndividualStudentId.Value &&
                                              x.Roles.Any(y => y.Name == "Student") &&
                                              x.StudyGroups.Any(y => y.Id == dto.StudyGroupId))
                    ?? throw new EntityNotFoundException("Ученик с таким Id не найден в переданной группе");

            var exercise = new Exercise
            {
                Teacher = teacher,
                StudyGroup = studyGroup,
                IndividualStudent = individualStudent,
                IsIndividual = dto.IsIndividual,
                Content = dto.Content,
                Lesson = lesson,
                DeadlineDateTime = dto.DeadlineDateTime,
            };

            await _db.Exercises.AddAsync(exercise);

            await _db.SaveChangesAsync();
        }
        
        public async Task DeleteExercise(long exerciseId)
        {
            var exercise = await _db.Exercises
                .Include(x => x.Files)
                .FirstOrDefaultAsync(x => x.Id == exerciseId);

            if (exercise != null)
            {
                _db.Exercises.Remove(exercise);

                await _fileService.DeleteFiles(exercise.Files
                    .Select(x => x.Id)
                    .ToList());
            }

            await _db.SaveChangesAsync();
        }

        public async Task<ExerciseFullInfoDto> GetExerciseFullInfo(long exerciseId, long userId, List<string> userRoles, long childId)
        {
            var exercise = await _db.Exercises
                .Include(x => x.StudyGroup)
                    .ThenInclude(x => x.Students)
                .Include(x => x.IndividualStudent)
                .Include(x => x.Teacher)
                .Include(x => x.Lesson)
                .Include(x => x.Marks)
                    .ThenInclude(x => x.Student)
                .Include(x => x.Files)
                .Include(x => x.Answers)
                    .ThenInclude(x => x.Files)
                .Include(x => x.Answers)
                    .ThenInclude(x => x.Student)
                .FirstOrDefaultAsync(x => x.Id == exerciseId)
                ?? throw new EntityNotFoundException("Задание с таким Id не найдено");

            long studentId = 0;

            if (userRoles.Contains("Parent"))
            {
                if (childId == 0)
                    throw new ArgumentException("Не выбран ребенок");

                studentId = childId;
            }
            else if (userRoles.Contains("Student"))
                studentId = userId;

            var exerciseDto = GetExerciseFullInfoDtoWithBaseInfo(exercise);

            if (exercise.Files.Count > 0)
                exerciseDto.Files = exercise.Files
                    .Select(x => new FileInfoDto(x))
                    .ToList();

            if (exercise.IsIndividual && exercise.IndividualStudentId.HasValue)
                exerciseDto.IndividualStudent = new ExerciseStudentInfoDto(exercise.IndividualStudent!);

            if (studentId != 0)
            {
                if (exercise.IndividualStudentId.HasValue && exercise.IndividualStudentId.Value != studentId)
                    throw new Exception("Посмотреть это индивидуальное задание может только тот ученик, которому оно выдано");

                var student = exercise.StudyGroup.Students
                    .FirstOrDefault(x => x.Id == studentId)
                    ?? throw new Exception("Посмотреть это задание может только ученик из группы, которой оно выдано");


                var studentDto = new ExerciseStudentInfoDto(student!);

                var studentAnswers = exercise.Answers
                    .Where(x => x.StudentId == studentId)
                    .Select(x => new ExerciseAnswerInfoDto
                    {
                        LessonId = exercise.LessonId,
                        Student = new UserShortInfoDto(x.Student),
                        ExerciseId = exerciseId,
                        ExerciseAnswerId = x.Id,
                        Content = x.Content,
                        Files = x.Files
                            .Select(y => new FileInfoDto(y))
                            .ToList(),
                    })
                    .ToList();

                var studentMarks = exercise.Marks
                    .Where(x => x.StudentId == studentId &&
                                x.MarkType == MarkType.ForExercise)
                    .Select(x => new MarkInfoDto
                    { 
                        MarkId = x.Id,
                        MarkType = x.MarkType.GetDescription(),
                        MarkValue = x.Value,
                        TeacherComment = x.Comment,
                    })
                    .ToList();

                studentDto.Marks = studentMarks;

                exerciseDto.Answers.AddRange(studentAnswers);
                exerciseDto.IndividualStudent = studentDto;
            }
            else
            {
                exerciseDto.Answers = exercise.Answers
                    .Select(x => new ExerciseAnswerInfoDto
                    {
                        LessonId = exercise.LessonId,
                        Student = new UserShortInfoDto(x.Student),
                        ExerciseId = exerciseId,
                        ExerciseAnswerId = x.Id,
                        Content = x.Content,
                        Files = x.Files
                            .Select(y => new FileInfoDto(y))
                            .ToList(),
                    })
                    .ToList();

                if (!exercise.IsIndividual)
                    exerciseDto.Students = exercise.StudyGroup.Students
                        .Select(x =>  new ExerciseStudentInfoDto(x)
                        {
                            Marks = exercise.Marks
                                .Where(y => y.StudentId == x.Id &&
                                            y.MarkType == MarkType.ForExercise)
                                .Select(y => new MarkInfoDto
                                {
                                    MarkId = y.Id,
                                    MarkValue = y.Value,
                                    MarkType = y.MarkType.ToString(),
                                    TeacherComment = y.Comment,
                                })
                                .ToList()
                        })
                        .ToList();
                else if (exercise.IndividualStudentId.HasValue)
                {
                    exerciseDto.IndividualStudent!.Marks = exercise.Marks
                        .Where(x => x.StudentId == exercise.IndividualStudentId.Value &&
                                    x.MarkType == MarkType.ForExercise)
                        .Select(x => new MarkInfoDto
                        {
                            MarkId = x.Id,
                            MarkValue = x.Value,
                            MarkType = x.MarkType.ToString(),
                            TeacherComment = x.Comment,
                        })
                        .ToList();
                }
            }

            return exerciseDto;
        }

        private ExerciseFullInfoDto GetExerciseFullInfoDtoWithBaseInfo(Exercise exercise)
        {
            var dto = new ExerciseFullInfoDto
            {
                ExerciseId = exercise.Id,
                Content = exercise.Content,
                IsIndividual = exercise.IsIndividual,
                LessonId = exercise.LessonId,
                StudyGroupId = exercise.StudyGroupId,
                StudyGroupName = exercise.StudyGroup.Name,
                Teacher = new UserShortInfoDto(exercise.Teacher),
                CreateDateTime = exercise.CreateDateTime,
                DeadlineDateTime = exercise.DeadlineDateTime,
            };

            return dto;
        }

        public async Task UpdateExercise(long exerciseId, UpdateExerciseDto dto)
        {
            var exercise = await _db.Exercises
                .Include(x => x.StudyGroup)
                .FirstOrDefaultAsync(x => x.Id == exerciseId)
                ?? throw new EntityNotFoundException("Задание с таким Id не найдено");

            if (dto.IsIndividual.HasValue)
            {
                User? individualStudent = null;
                
                if (dto.IsIndividual.Value)
                {
                    if (!dto.IndividualStudentId.HasValue)
                        throw new InvalidOperationException("Индивидуальное занятие должно быть кому то назначено");

                    individualStudent = await _db.Users
                        .FirstOrDefaultAsync(x => x.Id == dto.IndividualStudentId.Value &&
                                                  x.Roles.Any(y => y.Name == "Student") &&
                                                  x.StudyGroups.Any(y => y.Id == exercise.StudyGroupId))
                        ?? throw new EntityNotFoundException("Ученик с таким Id из текущей группы не найден");
                }

                exercise.IsIndividual = dto.IsIndividual.Value;
                exercise.IndividualStudent = individualStudent;
            }

            if (dto.TeacherId.HasValue)
            {
                var teacher = await _db.Users
                        .FirstOrDefaultAsync(x => x.Id == dto.TeacherId.Value &&
                                                  x.Roles.Any(y => y.Name == "Teacher"))
                        ?? throw new EntityNotFoundException("Преподаватель с таким Id не найден");

                exercise.Teacher = teacher;
            }

            if (!string.IsNullOrWhiteSpace(dto.Content))
                exercise.Content = dto.Content;

            if (dto.DeadlineDateTime.HasValue)
                exercise.DeadlineDateTime = dto.DeadlineDateTime.Value;

            await _db.SaveChangesAsync();
        }

        public async Task AttachFilesToExercise(long exerciseId, long userId, List<IFormFile> files)
        {
            await _fileService.UploadFiles<Exercise>(files, userId, FileLinkType.Exercise, exerciseId);
        }

        public async Task DetachFilesFromExercise(long exerciseId, List<long> fileIds)
        {
            await _fileService.DeleteFiles(fileIds);
        }
    }
}
