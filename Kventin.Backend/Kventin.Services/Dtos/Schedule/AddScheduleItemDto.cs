using Kventin.DataAccess.Enums;
using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.Schedule
{
    public class AddScheduleItemDto
    {
        [Required]
        public required DayOfTheWeek DayOfWeek { get; set; }

        [Required]
        public required TimeOnly StartTime { get; set; }

        [Required]
        public required TimeOnly EndTime { get; set; }
        public string? Classroom { get; set; }

        [Required]
        public required long TeacherId { get; set; }

        [Required]
        public required long GroupId { get; set; }

        [Required]
        public required long SubjectId { get; set; }

        [Required]
        public required long ScheduleId { get; set; }

        [Required]
        public required bool IsOnline { get; set; }
    }
}
