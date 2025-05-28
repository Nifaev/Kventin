using Kventin.DataAccess.Domain;
using Kventin.DataAccess.Enums;
using Kventin.Services.Dtos.Users;
using Kventin.Services.Infrastructure.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.Lessons
{
    public class LessonShortInfoDto
    {
        public LessonShortInfoDto(Lesson lesson) 
        {
            LessonId = lesson.Id;
            StartTime = lesson.StartTime;
            EndTime = lesson.EndTime;
            IsOnline = lesson.IsOnline;
            Classroom = lesson.Classroom;
            SubjectName = lesson.Subject.Name;
            StudyGroupName = lesson.StudyGroup.Name;
            Teacher = new UserShortInfoDto(lesson.Teacher);
            Date = lesson.Date;
            DayOfWeek = lesson.Date.DayOfWeek.MapToDayOfTheWeek();
            Status = lesson.Status.GetDescription();
        }

        [Required]
        public long LessonId { get; set; }

        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }

        [Required]
        public bool IsOnline { get; set; }

        public string? Classroom { get; set; }

        [Required]
        public string SubjectName { get; set; }

        [Required]
        public string StudyGroupName { get; set; }

        [Required]
        public UserShortInfoDto Teacher { get; set; }

        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public DayOfTheWeek DayOfWeek { get; set; }

        [Required]
        public string Status { get; set; }
    }
}
