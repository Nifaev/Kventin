namespace Kventin.Services.Dtos.Users
{
    public class UserShortInfoDto
    {
        public int UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? MiddleName { get; set; }
        public required string FullName { get; set; }
        public required string ShortName { get; set; }
    }
}
