namespace Kventin.Services.Interfaces.Tools
{
    public interface IPasswordHasher
    {
        public string Generate(string password);
        bool Verify(string password, string hashedPassword);
    }
}
