using Kventin.DataAccess;
using Kventin.DataAccess.Domain;
using Kventin.Services.Dtos.User;
using Kventin.Services.Infrastructure.Exceptions;
using Kventin.Services.Interfaces.Auth;
using Kventin.Services.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Kventin.Services.Services
{
    public class UserService(
        KventinContext db,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider) : IUserService
    {
        private readonly KventinContext _db = db;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly IJwtProvider _jwtProvider = jwtProvider;

        public async Task<string> Login(LoginUserDto dto)
        {
            var hashedPassword = string.Empty;
            var login = string.Empty;

            if (dto.PhoneNumber != null)
            {
                hashedPassword = await _db.Users
                    .Where(x => x.PhoneNumber.CompareTo(dto.PhoneNumber) == 0)
                    .Select(x => x.HashedPassword)
                    .FirstOrDefaultAsync()
                    ?? throw new EntityNotFoundException("Пользователь с таким номером телефона не зарегистрирован");

                login = dto.PhoneNumber;
            }
            else if (dto.Email != null)
            {
                hashedPassword = await _db.Users
                    .Where(x => x.Email != null &&
                                x.Email.CompareTo(dto.Email) == 0)
                    .Select(x => x.HashedPassword)
                    .FirstOrDefaultAsync()
                    ?? throw new EntityNotFoundException("Пользователь с такой эл. почтой не зарегистрирован");

                login = dto.Email;
            }
            else throw new ArgumentException("Ни номер телефона, ни эл. почта не были переданы");

            var verified = _passwordHasher.Verify(dto.Password, hashedPassword);

            if (!verified)
                throw new AuthException("Неправильный пароль");

            var token = _jwtProvider.GenerateToken(login);

            return token;
        }

        public async Task Register(RegisterUserDto dto)
        {
            var hashedPassword = _passwordHasher.Generate(dto.Password);

            var isUniquePhoneNumber = !await _db.Users
                .AnyAsync(x => x.PhoneNumber.CompareTo(dto.PhoneNumber) == 0);

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
                PhoneNumber = dto.PhoneNumber,
                HashedPassword = hashedPassword,
            };

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }
    }
}
