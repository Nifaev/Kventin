using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.Lessons
{
    public class SchoolWeekDto
    {
        [Required]
        public DateOnly StartOfWeek { get; set; }

        [Required]
        public DateOnly EndOFWeek { get; set; }

        public List<SchoolDayDto> SchoolDays { get; set; } = [];
    }
}
