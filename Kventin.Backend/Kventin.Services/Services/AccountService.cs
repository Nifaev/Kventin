using Kventin.DataAccess;
using Kventin.Services.Dtos.Users;
using Kventin.Services.Infrastructure.Exceptions;
using Kventin.Services.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Kventin.Services.Services
{
    public class AccountService(KventinContext db,
        IUserService userService,
        IAuthService authService) : IAccountService
    {
        private readonly KventinContext _db = db;
        private readonly IUserService _userService = userService;
        private readonly IAuthService _authService = authService;

        public async Task UpdateUserAccountInfo(IRequestCookieCollection cookies, UpdateUserAccountInfoDto dto, int userId, int authorizedUserId)
        {
            var authorizedUserRoles = _authService.GetUserRolesByCookie(cookies);

            // Если пользователь НЕ суперпользователь и НЕ Админ по ЛК,
            // то он может изменять только свой аккаунт
            if (!authorizedUserRoles.Contains("SuperUser") &&
                !authorizedUserRoles.Contains("AdminPersonalAccounts") &&
                userId != authorizedUserId)
                throw new NoAccessException("Вы можете редактировать только свой профиль");

            var user = await _db.Users.FindAsync(userId);

            if (user == null)
                throw new EntityNotFoundException("Пользователь с таким Id не найден");

            if (!string.IsNullOrWhiteSpace(dto.Email))
                user.Email = dto.Email;

            if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
                user.PhoneNumber = dto.PhoneNumber;

            if (!string.IsNullOrWhiteSpace(dto.ContractNumber))
                user.ContractNumber = dto.ContractNumber;

            if (!string.IsNullOrWhiteSpace(dto.VkLink))
                user.VkLink = dto.VkLink;

            if (!string.IsNullOrWhiteSpace(dto.TgLink))
                user.TgLink = dto.TgLink;

            await _db.SaveChangesAsync();
        }

        public async Task<UserAccountInfoDto> GetUserAccountInfo(int userId)
        {
            var user = await _db.Users.FindAsync(userId);

            if (user == null)
                throw new EntityNotFoundException("Пользователь с таким Id не найден");

            var result = new UserAccountInfoDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                MiddleName = user.MiddleName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ContractNumber = user.ContractNumber,
                VkLink = user.VkLink,
                TgLink = user.TgLink,
            };

            return result;
        }
    }
}
