namespace Kventin.Services.Dtos.Schedule
{
    public class ReturnScheduleDto
    {
        public int ScheduleId { get; set; }
        public List<ReturnScheduleItemDto> ScheduleItems { get; set; } = [];
    }
}
