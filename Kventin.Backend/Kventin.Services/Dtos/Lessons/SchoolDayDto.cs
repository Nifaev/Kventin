using Kventin.DataAccess.Enums;
using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.Lessons
{
    public class SchoolDayDto
    {
        [Required]
        public DateOnly Date { get; set; }
        
        [Required]
        public DayOfTheWeek DayOfWeek { get; set; }

        public List<LessonShortInfoDto> Lessons { get; set; } = [];
    }
}
