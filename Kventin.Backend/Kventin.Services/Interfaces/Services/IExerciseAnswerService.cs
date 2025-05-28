using Kventin.Services.Dtos.ExerciseAnswers;
using Microsoft.AspNetCore.Http;

namespace Kventin.Services.Interfaces.Services
{
    public interface IExerciseAnswerService
    {

        Task CreateExerciseAsnwer(long studentId, CreateExerciseAnswerDto dto);
        Task UpdateExerciseAnswer(long studentId, long answerId, string answerContent);
        Task DeleteExerciseAnswer(long studentId, long answerId);
        Task AttachFilesToExerciseAnswer(long studentId, long answerId, List<IFormFile> files);
        Task DetachFilesFromExerciseAnswer(long studentId, long answerId, List<long> fileIds);
    }
}
