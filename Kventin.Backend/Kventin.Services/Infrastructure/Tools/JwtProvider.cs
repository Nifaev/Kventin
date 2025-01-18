using Kventin.DataAccess.Domain;
using Kventin.Services.Dtos.User;
using Kventin.Services.Interfaces.Tools;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Kventin.Services.Infrastructure.Tools
{
    public class JwtProvider(IOptions<JwtOption> options) : IJwtProvider
    {
        private readonly JwtOption _options = options.Value;

        public string GenerateToken(int userId, string userLogin, List<Role> roles)
        {
            List<Claim> claims = 
            [
                new("userId", userId.ToString()),
                new("userLogin", userLogin)
            ];

            foreach (var role in roles)
                claims.Add(new Claim("role", role.Name));

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

        public UserIdDto GetUserIdByToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var userId = 0;

            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);

                userId = int.Parse(jwtToken.Claims.First().Value);
            }

            return new UserIdDto { UserId = userId };
        }
    }
}
