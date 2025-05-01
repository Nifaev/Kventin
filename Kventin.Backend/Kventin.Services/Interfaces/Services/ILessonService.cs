using Kventin.Services.Dtos.Lessons;

namespace Kventin.Services.Interfaces.Services
{
    public interface ILessonService
    {
        Task<GetLessonInfoForTeacherDto> GetTeacherLessonInfo(int lessonId, int userId); 
        Task<GetLessonInfoForStudentOrParentDto> GetStudentOrParentLessonInfo(int lessonId, int userId); 
    }
}
