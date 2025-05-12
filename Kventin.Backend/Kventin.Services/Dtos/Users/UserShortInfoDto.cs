using Kventin.DataAccess.Domain;
using Kventin.Services.Infrastructure.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.Users
{
    public class UserShortInfoDto
    {
        public UserShortInfoDto(User user)
        {
            UserId = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            MiddleName = user.MiddleName;
            FullName = user.GetFullName();
            ShortName = user.GetShortName();
        }

        [Required]
        public int UserId { get; set; }
        
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string? MiddleName { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string ShortName { get; set; }
    }
}
