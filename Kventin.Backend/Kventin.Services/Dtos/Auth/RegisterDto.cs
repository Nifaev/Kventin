using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.Auth
{
    public class RegisterDto
    {
        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        public string? MiddleName { get; set; }

        [Required]
        [Phone]
        public required string PhoneNumber { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        public required string PasswordConfirmation { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
    }
}
