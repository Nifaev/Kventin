using Kventin.DataAccess;
using Kventin.DataAccess.Domain;
using Kventin.Services.Dtos.Auth;
using Kventin.Services.Dtos.Users;
using Kventin.Services.Infrastructure.Exceptions;
using Kventin.Services.Interfaces.Services;
using Kventin.Services.Interfaces.Tools;
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

        public async Task<string> Login(LoginDto dto)
        {
            var hashedPassword = string.Empty;
            var login = string.Empty;
            var userId = 0;
            var roles = new List<Role>();

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
                roles = userData.Roles;
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
                roles = userData.Roles;
                userId = userData.Id;
                login = dto.Email;
            }
            else throw new ArgumentException("Ни номер телефона, ни эл. почта не были переданы");

            var verified = _passwordHasher.Verify(dto.Password, hashedPassword);

            if (!verified)
                throw new AuthException("Неправильный пароль");

            var token = _jwtProvider.GenerateToken(userId, login, roles);

            return token;
        }

        public async Task Register(RegisterDto dto)
        {
            var hashedPassword = _passwordHasher.Generate(dto.Password);

            var shortPhoneNumber = GetShortPhoneNumber(dto.PhoneNumber);

            var isUniquePhoneNumber = !await _db.Users
                .AnyAsync(x => x.PhoneNumber.CompareTo(shortPhoneNumber) == 0);

            bool isUniqueEmail = dto.Email == null ||
                                 !await _db.Users.AnyAsync(x => x.Email != null &&
                                                                x.Email.CompareTo(dto.Email) == 0);

            if (!isUniquePhoneNumber)
                throw new AuthException("Пользователь с таким номером телефона уже существует");

            if (!isUniqueEmail)
                throw new AuthException("Пользователь с такой эл. почтой уже существует");

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                MiddleName = dto.MiddleName,
                Email = dto.Email,
                PhoneNumber = shortPhoneNumber,
                HashedPassword = hashedPassword,
            };

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }

        public UserIdDto GetUserIdByToken(string token)
        {
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
    }
}
