using Kventin.Services.Dtos.Users;
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

        public string GenerateToken(int userId, string userLogin, List<string> rolenames, int selectedChildId = 0)
        {
            List<Claim> claims = 
            [
                new("userId", userId.ToString(), ClaimValueTypes.Integer),
                new("userLogin", userLogin),
                new("selectedChild", selectedChildId.ToString(), ClaimValueTypes.Integer)
            ];

            foreach (var rolename in rolenames)
                claims.Add(new Claim("role", rolename));

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

        public UserIdDto GetChildIdByToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var userId = 0;

            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);

                userId = int.Parse(jwtToken.Claims.First(x => x.Type == "childId").Value);
            }

            return new UserIdDto { UserId = userId };
        }

        public UserIdDto GetUserIdByToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var userId = 0;

            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);

                userId = int.Parse(jwtToken.Claims.First(x => x.Type == "userId").Value);
            }

            return new UserIdDto { UserId = userId };
        }

        public string GetUserLoginByToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var login = string.Empty;

            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);

                login = jwtToken.Claims.First(x => x.Type == "userLogin").Value;
            }

            return login;
        }

        public List<UserRoleDto> GetUserRolesByToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var roles = new List<UserRoleDto>();

            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);

                roles = jwtToken.Claims
                    .Where(x => x.Type == "role")
                    .Select(x =>  new UserRoleDto
                    {
                        RoleName = x.Value
                    })
                    .ToList();
            }

            return roles;
        }
    }
}
