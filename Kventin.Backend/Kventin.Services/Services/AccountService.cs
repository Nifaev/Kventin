using Kventin.DataAccess;
using Kventin.Services.Dtos.Users;
using Kventin.Services.Infrastructure.Exceptions;
using Kventin.Services.Infrastructure.Extensions;
using Kventin.Services.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Kventin.Services.Services
{
    public class AccountService(KventinContext db,
        IUserService userService) : IAccountService
    {
        private readonly KventinContext _db = db;
        private readonly IUserService _userService = userService;

        public async Task UpdateUserAccountInfo(UpdateUserAccountInfoDto dto, int userId, int authorizedUserId)
        {
            var authorizedUserRoles = await _userService.GetUserRoles(authorizedUserId);

            // Если пользователь НЕ суперпользователь и НЕ Админ по ЛК,
            // то он может изменять только свой аккаунт
            if (!authorizedUserRoles.Select(x => x.RoleName).Contains("SuperUser") &&
                !authorizedUserRoles.Select(x => x.RoleName).Contains("AdminPersonalAccounts") &&
                userId != authorizedUserId)
                throw new NoAccessException("Вы можете редактировать только свой профиль");

            var user = await _db.Users.FindAsync(userId);

            if (user == null)
                throw new EntityNotFoundException("Пользователь с таким Id не найден");

            if (dto.Email != null)
                user.Email = dto.Email;

            if (dto.PhoneNumber != null)
                user.PhoneNumber = dto.PhoneNumber;

            if (dto.ContractNumber != null)
                user.ContractNumber = dto.ContractNumber;

            if (dto.VkLink != null)
                user.VkLink = dto.VkLink;

            if (dto.TgLink != null)
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
