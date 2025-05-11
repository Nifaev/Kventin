namespace Kventin.Services.Dtos.Schedule
{
    public class ReturnScheduleDto
    {
        public required int ScheduleId { get; set; }
        public List<ReturnScheduleItemDto> ScheduleItems { get; set; } = [];
    }
}
