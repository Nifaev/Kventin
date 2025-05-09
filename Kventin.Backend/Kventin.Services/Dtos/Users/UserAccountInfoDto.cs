namespace Kventin.Services.Dtos.Users
{
    public class UserAccountInfoDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? VkLink { get; set; }
        public string? TgLink { get; set; }
        public string? ContractNumber { get; set; }
    }
}
