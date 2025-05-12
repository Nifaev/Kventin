using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.Schedule
{
    public class ScheduleDto
    {
        [Required]
        public required int StartYear { get; set; }

        [Required]
        public required int EndYear { get; set; }
    }
}
