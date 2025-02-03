using Kventin.Services.Dtos.Schedule;

namespace Kventin.Services.Interfaces.Services
{
    public interface IScheduleService
    {
        Task<List<ReturnScheduleItemDto>> GetSchedule(ScheduleDto dto);
        Task AddScheduleItem(AddScheduleItemDto dto);
        Task UpdateScheduleItem(UpdateScheduleItemDto dto, int itemId);
        Task DeleteScheduleItem(int itemId);
        Task CreateSchedule(ScheduleDto dto);
    }
}
