using Kventin.Services.Dtos.Marks;

namespace Kventin.Services.Interfaces.Services
{
    public interface IMarkService
    {
        Task AssignMarksForLesson(int teacherId, AssignMarksForLessonDto dto);
        Task AssignMarksForExercise(int teacherId, AssignMarksForExerciseDto dto);
        Task UpdateMark(int markId, MarkShortInfoDto dto);
        Task DeleteMark(int markId);
    }
}
