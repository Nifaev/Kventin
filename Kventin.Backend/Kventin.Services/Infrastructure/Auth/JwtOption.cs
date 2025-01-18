namespace Kventin.Services.Infrastructure.Auth
{
    public class JwtOption
    {
        public byte[] SecretKey { get; set; } = [];
        public int ExpiresHours { get; set; }
    }
}
