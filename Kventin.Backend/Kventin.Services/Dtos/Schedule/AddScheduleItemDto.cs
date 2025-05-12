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
        public required int TeacherId { get; set; }

        [Required]
        public required int GroupId { get; set; }

        [Required]
        public required int SubjectId { get; set; }

        [Required]
        public required int ScheduleId { get; set; }

        [Required]
        public required bool IsOnline { get; set; }
    }
}
