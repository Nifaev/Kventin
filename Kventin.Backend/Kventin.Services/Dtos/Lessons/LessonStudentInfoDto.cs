using Kventin.DataAccess.Domain;
using Kventin.Services.Dtos.Marks;
using Kventin.Services.Dtos.Users;
using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.Lessons
{
    public class LessonStudentInfoDto : UserShortInfoDto
    {
        public LessonStudentInfoDto(User user) : base(user) { }

        /// <summary>
        /// Оценки за занятие (без заданий)
        /// </summary>
        public List<MarkInfoDto> Marks { get; set; } = [];

        [Required]
        public bool Attended { get; set; }
    }
}
