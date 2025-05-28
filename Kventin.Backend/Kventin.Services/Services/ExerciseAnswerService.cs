using Kventin.DataAccess;
using Kventin.DataAccess.Domain;
using Kventin.DataAccess.Enums;
using Kventin.Services.Dtos.ExerciseAnswers;
using Kventin.Services.Infrastructure.Exceptions;
using Kventin.Services.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Kventin.Services.Services
{
    public class ExerciseAnswerService(KventinContext db,
        IFileService fileService) : IExerciseAnswerService
    {
        private readonly KventinContext _db = db;
        private readonly IFileService _fileService = fileService;

        public async Task CreateExerciseAsnwer(long studentId, CreateExerciseAnswerDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Content))
                throw new ArgumentException("Поле Content не должно быть null или пустой строкой");

            var student = await _db.Users
                .FirstOrDefaultAsync(x => x.Id == studentId &&
                                          x.Roles.Any(y => y.Name == "Student"))
                ?? throw new EntityNotFoundException("Данный метод доступен только ученикам");

            var exercise = await _db.Exercises
                .Include(x => x.IndividualStudent)
                .Include(x => x.StudyGroup)
                    .ThenInclude(x => x.Students)
                .FirstOrDefaultAsync(x => x.Id == dto.ExerciseId)
                ?? throw new EntityNotFoundException("Заданеи с таким Id не найдено");

            if (exercise.IsIndividual && exercise.IndividualStudentId != studentId)
                throw new NoAccessException("Вы не можете дать ответ на это задание, т.к. оно являвется индивидуальным и выдано не вам");

            if (!exercise.IsIndividual && !exercise.StudyGroup.Students.Any(x => x.Id == studentId))
                throw new NoAccessException("Вы не можете дать ответ на это задание, т.к. оно выдано не вашей группе");

            var answer = new ExerciseAnswer
            { 
                Content = dto.Content,
                Exercise = exercise,
                Student = student,
            };

            await _db.ExerciseAnswers.AddAsync(answer);

            await _db.SaveChangesAsync();
        }

        public async Task DeleteExerciseAnswer(long studentId, long answerId)
        {
            var student = await _db.Users
                .FirstOrDefaultAsync(x => x.Id == studentId &&
                                          x.Roles.Any(y => y.Name == "Student"))
                ?? throw new EntityNotFoundException("Данный метод доступен только ученикам");

            var exerciseAnswer = await _db.ExerciseAnswers
                .Include(x => x.Files)
                .FirstOrDefaultAsync(x => x.Id == answerId &&
                                          x.StudentId == studentId)
                ?? throw new EntityNotFoundException("Ваш ответ на задание с таким Id не найден");

            var fileIds = exerciseAnswer.Files
                .Select(x => x.Id)
                .ToList();

            await _fileService.DeleteFiles(fileIds);

            _db.ExerciseAnswers.Remove(exerciseAnswer);

            await _db.SaveChangesAsync();
        }

        public async Task UpdateExerciseAnswer(long studentId, long answerId, string answerContent)
        {
            var student = await _db.Users
                .FirstOrDefaultAsync(x => x.Id == studentId &&
                                          x.Roles.Any(y => y.Name == "Student"))
                ?? throw new EntityNotFoundException("Данный метод доступен только ученикам");

            var exerciseAnswer = await _db.ExerciseAnswers
                .Include(x => x.Files)
                .FirstOrDefaultAsync(x => x.Id == answerId &&
                                          x.StudentId == studentId)
                ?? throw new EntityNotFoundException("Ваш ответ на задание с таким Id не найден");

            exerciseAnswer.Content = answerContent;

            await _db.SaveChangesAsync();
        }

        public async Task DetachFilesFromExerciseAnswer(long studentId, long answerId, List<long> fileIds)
        {
            await CheckAccess(studentId, answerId);

            await _fileService.DeleteFiles(fileIds);
        }

        public async Task AttachFilesToExerciseAnswer(long studentId, long answerId, List<IFormFile> files)
        {
            await CheckAccess(studentId, answerId);

            await _fileService.UploadFiles<ExerciseAnswer>(files, studentId, FileLinkType.ExerciseAnswer, answerId);
        }

        private async Task CheckAccess(long studentId, long answerId)
        {
            var isStudent = await _db.Users
                .AnyAsync(x => x.Id == studentId &&
                               x.Roles.Any(y => y.Name == "Student"));

            if (!isStudent)
                throw new EntityNotFoundException("Данный метод доступен только ученикам");

            var hasAccess = await _db.ExerciseAnswers
                .AnyAsync(x => x.Id == answerId &&
                               x.StudentId == studentId);

            if (!hasAccess)
                throw new EntityNotFoundException("Ваш ответ на задание с таким Id не найден");
        }
    }
}
