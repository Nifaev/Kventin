using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.Users
{
    public class UpdateUserAccountInfoDto
    {
        [EmailAddress]
        public string? Email { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        public string? VkLink { get; set; }
        public string? TgLink { get; set; }
        public string? ContractNumber { get; set; }
    }
}
