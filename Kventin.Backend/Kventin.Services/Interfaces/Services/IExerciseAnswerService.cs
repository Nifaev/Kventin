using Kventin.Services.Dtos.ExerciseAnswers;
using Microsoft.AspNetCore.Http;

namespace Kventin.Services.Interfaces.Services
{
    public interface IExerciseAnswerService
    {

        Task CreateExerciseAsnwer(int studentId, CreateExerciseAnswerDto dto);
        Task UpdateExerciseAnswer(int studentId, int answerId, string answerContent);
        Task DeleteExerciseAnswer(int studentId, int answerId);
        Task AttachFilesToExerciseAnswer(int studentId, int answerId, List<IFormFile> files);
        Task DetachFilesFromExerciseAnswer(int studentId, int answerId, List<int> fileIds);
    }
}
