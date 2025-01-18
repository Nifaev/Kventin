namespace Kventin.Services.Interfaces.Auth
{
    public interface IPasswordHasher
    {
        public string Generate(string password);
        bool Verify(string password, string hashedPassword);
    }
}
