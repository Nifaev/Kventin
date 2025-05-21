using Kventin.Services.Dtos.Marks;
using Kventin.Services.Dtos.Users;
using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.Exercises
{
    public class ExerciseShortInfoDto
    {
        [Required]
        public required int ExeriseId { get; set; }

        [Required]
        public DateTime CreateDateTime { get; set; }

        public DateTime? DeadlineDateTime { get; set; }

        [Required]
        public required bool IsIndividual { get; set; }

        public UserShortInfoDto? IndividualStudent { get; set; }

        /// <summary>
        /// Это поле будет заполнено только для ученика и родителя (если оценка есть)
        /// </summary>
        public MarkInfoDto? Mark { get; set; }
    }
}
