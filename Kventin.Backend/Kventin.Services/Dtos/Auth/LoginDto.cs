using Kventin.Services.Infrastructure.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.Auth
{
    public class LoginDto
    {
        [Required]
        public required string Password { get; set; }

        [OneOfTwoRequired("PhoneNumber", "Email")]
        //[Phone]
        public string? PhoneNumber { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
    }
}
