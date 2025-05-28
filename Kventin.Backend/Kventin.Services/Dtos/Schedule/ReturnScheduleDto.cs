using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.Schedule
{
    public class ReturnScheduleDto
    {
        [Required]
        public required long ScheduleId { get; set; }
        public List<ReturnScheduleItemDto> ScheduleItems { get; set; } = [];
    }
}
