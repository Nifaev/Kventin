using Kventin.DataAccess.Domain;

namespace Kventin.Services.Infrastructure.Extensions
{
    public static class UserExtensions
    {
        public static string GetShortName(this User user)
        {
            var shortName = $"{user.LastName} {user.FirstName[0]}.{(!string.IsNullOrWhiteSpace(user.MiddleName) ? $" {user.MiddleName[0]}." : string.Empty)}";

            return shortName;
        }

        public static string GetFullName(this User user)
        {
            var fullName = $"{user.LastName} {user.FirstName}{(!string.IsNullOrWhiteSpace(user.MiddleName) ? $" {user.MiddleName}" : string.Empty)}";

            return fullName;
        }
    }
}
