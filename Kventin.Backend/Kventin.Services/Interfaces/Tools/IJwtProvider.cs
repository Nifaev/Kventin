namespace Kventin.Services.Interfaces.Tools
{
    public interface IJwtProvider
    {
        public string GenerateToken(int userId, string userLogin, List<string> rolenames, int selectedChildId = 0);

        public int GetUserIdByToken(string token);

        public List<string> GetUserRolesByToken(string token);

        public string GetUserLoginByToken(string token);

        public int GetChildIdByToken(string token);
    }
}
