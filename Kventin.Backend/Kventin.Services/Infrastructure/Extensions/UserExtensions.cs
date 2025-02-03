using Kventin.DataAccess.Domain;

namespace Kventin.Services.Infrastructure.Extensions
{
    public static class UserExtensions
    {
        public static string GetShortName(this User user)
        {
            var shortName = $"{user.LastName} {user.FirstName[0]}.";

            if (user.MiddleName == null)
                return shortName;
            else
                shortName += $" {user.MiddleName[0]}.";

            return shortName;
        }
    }
}
