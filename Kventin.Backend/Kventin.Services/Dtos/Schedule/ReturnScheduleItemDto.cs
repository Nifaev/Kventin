using Kventin.Services.Dtos.Users;
using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.Schedule
{
    public class ReturnScheduleItemDto
    {
        [Required]
        public long ScheduleItemId { get; set; }

        [Required]
        public required string DayOfWeek { get; set; }

        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }

        public string? Classroom { get; set; }

        [Required]
        public required UserShortInfoDto Teacher { get; set; }

        [Required]
        public required string GroupName { get; set; }

        [Required]
        public required string SubjectName { get; set; }

        [Required]
        public bool IsOnline { get; set; }
    }
}
