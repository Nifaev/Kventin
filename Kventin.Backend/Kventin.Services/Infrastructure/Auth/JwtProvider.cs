using Kventin.Services.Interfaces.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Kventin.Services.Infrastructure.Auth
{
    public class JwtProvider(IOptions<JwtOption> options) : IJwtProvider
    {
        private readonly JwtOption _options = options.Value;

        public string GenerateToken(string userLogin)
        {
            Claim[] claims = [new("userLogin", userLogin)];

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(_options.SecretKey),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.Now.AddHours(_options.ExpiresHours));

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }
    }
}
