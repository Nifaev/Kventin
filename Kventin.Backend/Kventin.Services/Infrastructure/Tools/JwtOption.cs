namespace Kventin.Services.Infrastructure.Tools
{
    public class JwtOption
    {
        public byte[] SecretKey { get; set; } = [];
        public int ExpiresHours { get; set; }
    }
}
