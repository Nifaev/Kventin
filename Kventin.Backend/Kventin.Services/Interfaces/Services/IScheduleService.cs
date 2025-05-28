using Kventin.Services.Dtos.Schedule;

namespace Kventin.Services.Interfaces.Services
{
    public interface IScheduleService
    {
        Task<ReturnScheduleDto> GetSchedule(ScheduleDto dto);
        Task AddScheduleItem(AddScheduleItemDto dto);
        Task UpdateScheduleItem(UpdateScheduleItemDto dto, long itemId);
        Task DeleteScheduleItem(long itemId);
        Task CreateSchedule(ScheduleDto dto);
        Task<List<string>> GetSchoolYears();
    }
}
