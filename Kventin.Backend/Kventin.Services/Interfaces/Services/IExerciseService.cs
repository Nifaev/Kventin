using Kventin.Services.Dtos.Exercises;
using Microsoft.AspNetCore.Http;

namespace Kventin.Services.Interfaces.Services
{
    public interface IExerciseService
    {
        Task CreateExercise(int teacherId, CreateExerciseDto dto);
        Task DeleteExercise(int exerciseId);
        Task UpdateExercise(int exerciseId, UpdateExerciseDto dto);
        Task<ExerciseFullInfoDto> GetExerciseFullInfo(int exerciseId, int userId, List<string> userRoles, int childId);
        Task AttachFilesToExercise(int exerciseId, int userId, List<IFormFile> files);
        Task DetachFilesFromExercise(int exerciseId, List<int> fileIds);
    }
}
