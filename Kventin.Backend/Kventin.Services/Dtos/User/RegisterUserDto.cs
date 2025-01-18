using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.User
{
    public class RegisterUserDto
    {
        [Required]
        public required string FirstName { get; set; }
        
        [Required]
        public required string LastName { get; set; }
        
        public string? MiddleName { get; set; }

        [Required]
        [Length(10, 10)]
        public required string PhoneNumber { get; set; }

        [Required]
        public required string Password { get; set; }
        
        [EmailAddress]
        public string? Email { get; set; }
    }
}
