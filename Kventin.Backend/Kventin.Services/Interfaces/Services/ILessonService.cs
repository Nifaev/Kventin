using Kventin.Services.Dtos.Lessons;
using Kventin.Services.Dtos.Marks;
using Microsoft.AspNetCore.Http;

namespace Kventin.Services.Interfaces.Services
{
    public interface ILessonService
    {
        Task<SchoolWeekDto> GetSchoolWeek(int skipWeeksCount, long userId, List<string> userRoles, long childId);
        Task<SchoolDayDto> GetSchoolDay(DateOnly date, long userId, List<string> userRoles, long childId);
        Task<LessonFullInfoDto> GetLessonFullInfo(long lessonId, long userId, List<string> userRoles, long childId);
        Task UpdateLesson(long lessonId, UpdateLessonDto dto);
        Task AttachFilesToLesson(long lessonId, List<IFormFile> files, long uploadedByUserId);
        Task DetachFilesFromLesson(long lessonId, List<long> fileIds);
        Task MarkAttendance(long lessonId, List<long> studentIds);
    }
}
