using Kventin.Services.Dtos.Exercises;
using Microsoft.AspNetCore.Http;

namespace Kventin.Services.Interfaces.Services
{
    public interface IExerciseService
    {
        Task CreateExercise(long teacherId, CreateExerciseDto dto);
        Task DeleteExercise(long exerciseId);
        Task UpdateExercise(long exerciseId, UpdateExerciseDto dto);
        Task<ExerciseFullInfoDto> GetExerciseFullInfo(long exerciseId, long userId, List<string> userRoles, long childId);
        Task AttachFilesToExercise(long exerciseId, long userId, List<IFormFile> files);
        Task DetachFilesFromExercise(long exerciseId, List<long> fileIds);
    }
}
