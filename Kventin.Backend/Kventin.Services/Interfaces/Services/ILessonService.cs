using Kventin.Services.Dtos.Lessons;
using Kventin.Services.Dtos.Marks;
using Microsoft.AspNetCore.Http;

namespace Kventin.Services.Interfaces.Services
{
    public interface ILessonService
    {
        Task<SchoolWeekDto> GetSchoolWeek(int skipWeeksCount, int userId, List<string> userRoles, int childId);
        Task<SchoolDayDto> GetSchoolDay(DateOnly date, int userId, List<string> userRoles, int childId);
        Task<LessonFullInfoDto> GetLessonFullInfo(int lessonId, int userId, List<string> userRoles, int childId);
        Task UpdateLesson(int lessonId, UpdateLessonDto dto);
        Task AttachFilesToLesson(int lessonId, List<IFormFile> files, int uploadedByUserId);
        Task DetachFilesFromLesson(int lessonId, List<int> fileIds);
        Task MarkAttendance(int lessonId, List<int> studentIds);
    }
}
