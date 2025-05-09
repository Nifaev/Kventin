using Kventin.Services.Dtos.Users;
using Kventin.Services.Interfaces.Services;
using Kventin.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kventin.WebApi.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController(IAccountService accountService,
        IAuthService authService) : ControllerBase
    {
        private readonly IAccountService _accountService = accountService;
        private readonly IAuthService _authService = authService;

        /// <summary>
        /// Получить информацию для личного кабинета
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Возвращает UserAccountInfoDto</returns>
        [HttpGet("{userId}/getAccountInfo")]
        [Authorize]
        public async Task<ActionResult<UserAccountInfoDto>> GetUserAccountInfo(int userId)
        {
            var result = await _accountService.GetUserAccountInfo(userId);

            return Ok(result);
        }

        /// <summary>
        /// Обновить информацию о пользователе
        /// (Superuser может редактировать всех пользователей,
        /// AdminPersonalAccounts может редактировать всех, кроме Superuser,
        /// Teacher/Student/Parent могут редактировать только свой профиль)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="dto">Передавать UpdateUserAccountInfoDto</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("{userId}/updateAccountInfo")]
        public async Task<ActionResult> UpdateUserAccountInfo(int userId, UpdateUserAccountInfoDto dto)
        {
            var authorizedUserId = _authService.GetUserIdByCookie(Request.Cookies);

            await _accountService.UpdateUserAccountInfo(dto, userId, authorizedUserId.UserId);

            return Ok();
        }

        /// <summary>
        /// Получить детей родителя
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Parent")]
        [HttpGet("{userId}/getChildren")]
        public async Task<ActionResult<GetUsersChildrenDto>> GetChildren(int id)
        {
            var result = await _accountService.GetUsersChildren(id);

            return Ok(result);
        }
    }
}
