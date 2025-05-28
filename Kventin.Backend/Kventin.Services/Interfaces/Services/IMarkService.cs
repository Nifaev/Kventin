using Kventin.Services.Dtos.Marks;

namespace Kventin.Services.Interfaces.Services
{
    public interface IMarkService
    {
        Task AssignMarksForLesson(long teacherId, AssignMarksForLessonDto dto);
        Task AssignMarksForExercise(long teacherId, AssignMarksForExerciseDto dto);
        Task UpdateMark(long markId, MarkShortInfoDto dto);
        Task DeleteMark(long markId);
    }
}
