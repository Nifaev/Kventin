using Kventin.DataAccess;
using Kventin.DataAccess.Domain;
using Kventin.Services.Dtos.Auth;
using Kventin.Services.Infrastructure.Exceptions;
using Kventin.Services.Interfaces.Services;
using Kventin.Services.Interfaces.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Kventin.Services.Services
{
    public class AuthService(
        KventinContext db,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider) : IAuthService
    {
        private readonly KventinContext _db = db;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly IJwtProvider _jwtProvider = jwtProvider;

        public async Task<string> GetNewCookieWithChildId(IRequestCookieCollection cookie, long parentId, long childId)
        {
            var parent = await _db.Users
                .FirstOrDefaultAsync(x => x.Id == parentId &&
                                          x.Children.Any(y => y.Id == childId));

            if (parent == null)
                throw new EntityNotFoundException("Родитель с таким Id не является родителем переданного ребенка");

            var token = cookie["choco-cookies"] ?? string.Empty;

            var userLogin = _jwtProvider.GetUserLoginByToken(token);

            var userRoles = _jwtProvider.GetUserRolesByToken(token);

            if (userLogin != null && userRoles != null)
            {
                token = _jwtProvider.GenerateToken(parentId, userLogin, userRoles, childId);
            }

            return token;
        }

        public async Task<string> Login(LoginDto dto)
        {
            var hashedPassword = string.Empty;
            var login = string.Empty;
            long userId = 0;
            var rolenames = new List<string>();

            if (dto.PhoneNumber != null)
            {
                var shortPhoneNumber = GetShortPhoneNumber(dto.PhoneNumber);

                var userData = await _db.Users
                    .Include(x => x.Roles)
                    .Where(x => x.PhoneNumber.CompareTo(shortPhoneNumber) == 0)
                    .Select(x => new { x.HashedPassword, x.Roles, x.Id })
                    .FirstOrDefaultAsync()
                    ?? throw new EntityNotFoundException("Пользователь с таким номером телефона не зарегистрирован");

                hashedPassword = userData.HashedPassword;
                rolenames = userData.Roles.Select(x => x.Name).ToList();
                login = shortPhoneNumber;
                userId = userData.Id;
            }
            else if (dto.Email != null)
            {
                var userData = await _db.Users
                    .Include(x => x.Roles)
                    .Where(x => x.Email != null &&
                                x.Email.CompareTo(dto.Email) == 0)
                    .Select(x => new { x.HashedPassword, x.Roles, x.Id })
                    .FirstOrDefaultAsync()
                    ?? throw new EntityNotFoundException("Пользователь с такой эл. почтой не зарегистрирован");

                hashedPassword = userData.HashedPassword;
                rolenames = userData.Roles.Select(x => x.Name).ToList();
                userId = userData.Id;
                login = dto.Email;
            }
            else throw new ArgumentException("Ни номер телефона, ни эл. почта не были переданы");

            var verified = _passwordHasher.Verify(dto.Password, hashedPassword);

            if (!verified)
                throw new ArgumentException("Неправильный пароль");

            if (!rolenames.Any())
                throw new AuthException("Дождитесь, пока администратор подтвердит вашу регистрацию");

            var token = _jwtProvider.GenerateToken(userId, login, rolenames);

            return token;
        }

        public async Task Register(RegisterDto dto)
        {
            if (dto.Password.CompareTo(dto.PasswordConfirmation) != 0)
                throw new ArgumentException("Пароли не совпадают");

            var hashedPassword = _passwordHasher.Generate(dto.Password);

            var shortPhoneNumber = GetShortPhoneNumber(dto.PhoneNumber);

            var isUniquePhoneNumber = !await _db.Users
                .AnyAsync(x => x.PhoneNumber.CompareTo(shortPhoneNumber) == 0);

            bool isUniqueEmail = dto.Email == null ||
                                 !await _db.Users.AnyAsync(x => x.Email != null &&
                                                                x.Email.CompareTo(dto.Email) == 0);

            if (!isUniquePhoneNumber)
                throw new ArgumentException("Пользователь с таким номером телефона уже существует");

            if (!isUniqueEmail)
                throw new ArgumentException("Пользователь с такой эл. почтой уже существует");

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                MiddleName = !string.IsNullOrWhiteSpace(dto.MiddleName) 
                    ? dto.MiddleName
                    : null,
                Email = !string.IsNullOrWhiteSpace(dto.Email)
                    ? dto.Email
                    : null,
                PhoneNumber = shortPhoneNumber,
                HashedPassword = hashedPassword,
            };

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }

        public long GetUserIdByCookie(IRequestCookieCollection cookie)
        {
            var token = cookie["choco-cookies"] ?? string.Empty;

            var userId = _jwtProvider.GetUserIdByToken(token);

            return userId;
        }

        private string GetShortPhoneNumber(string phoneNumber)
        {
            string shortPhoneNumber = string.Empty;

            if (phoneNumber.Length == 10)
                shortPhoneNumber = phoneNumber;

            if (phoneNumber.Length == 11)
                shortPhoneNumber = phoneNumber.Substring(1);

            if (phoneNumber.Length == 12)
                shortPhoneNumber = phoneNumber.Substring(2);

            return shortPhoneNumber;
        }

        public List<string> GetUserRolesByCookie(IRequestCookieCollection cookie)
        {
            var token = cookie["choco-cookies"] ?? string.Empty;

            var roles = _jwtProvider.GetUserRolesByToken(token);

            return roles;
        }

        public long GetChildIdByCookie(IRequestCookieCollection cookie)
        {
            var token = cookie["choco-cookies"] ?? string.Empty;

            var childId = _jwtProvider.GetChildIdByToken(token);

            return childId;
        }
    }
}
