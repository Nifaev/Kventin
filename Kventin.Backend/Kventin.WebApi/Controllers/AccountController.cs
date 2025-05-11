using Kventin.Services.Dtos.Users;
using Kventin.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kventin.WebApi.Controllers
{
    [ApiController]
    [Route("api/account")]
    [Authorize]
    public class AccountController(IAccountService accountService,
        IAuthService authService) : ControllerBase
    {
        private readonly IAccountService _accountService = accountService;
        private readonly IAuthService _authService = authService;

        /// <summary>
        /// Получить информацию для личного кабинета (Все авторизованные пользователи)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Возвращает UserAccountInfoDto</returns>
        [HttpGet("{userId}/getAccountInfo")]
        public async Task<ActionResult<UserAccountInfoDto>> GetUserAccountInfo(int userId)
        {
            var result = await _accountService.GetUserAccountInfo(userId);

            return Ok(result);
        }

        /// <summary>
        /// Обновить информацию о пользователе
        /// (Все авторизованные пользователи, 
        /// Superuser может редактировать всех пользователей,
        /// AdminPersonalAccounts может редактировать всех, кроме SuperUser,
        /// Teacher/Student/Parent могут редактировать только свой профиль)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="dto">Передавать UpdateUserAccountInfoDto</param>
        /// <returns></returns>
        [HttpPost("{userId}/updateAccountInfo")]
        public async Task<ActionResult> UpdateUserAccountInfo(int userId, UpdateUserAccountInfoDto dto)
        {
            var authorizedUserId = _authService.GetUserIdByCookie(Request.Cookies);

            await _accountService.UpdateUserAccountInfo(Request.Cookies, dto, userId, authorizedUserId);

            return Ok();
        }
    }
}
