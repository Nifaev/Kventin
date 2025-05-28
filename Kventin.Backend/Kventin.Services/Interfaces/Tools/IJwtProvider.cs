namespace Kventin.Services.Interfaces.Tools
{
    public interface IJwtProvider
    {
        public string GenerateToken(long userId, string userLogin, List<string> rolenames, long selectedChildId = 0);

        public long GetUserIdByToken(string token);

        public List<string> GetUserRolesByToken(string token);

        public string GetUserLoginByToken(string token);

        public long GetChildIdByToken(string token);
    }
}
