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
        Task AttachFileToLesson(int lessonId, IFormFile file, int uploadedByUserId);
        Task DetachFileFromLesson(int lessonId, int fileId);
        Task MarkAttendance(int lessonId, List<int> studentIds);
    }
}
