namespace Kventin.Services.Interfaces.Auth
{
    public interface IJwtProvider
    {
        public string GenerateToken(string userLogin);
    }
}
