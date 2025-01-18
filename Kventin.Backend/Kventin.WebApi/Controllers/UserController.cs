using Kventin.Services.Dtos.User;
using Kventin.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kventin.WebApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController(
        IUserService userService,
        IAuthService authService) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly IAuthService _authService = authService;

        /// <summary>
        /// Назначить роль пользователю (SuperUser, AdminRegistration)
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="dto">Принимает UserRoleDto</param>
        /// <returns></returns>
        [Authorize(Roles = "SuperUser, AdminRegistration")]
        [HttpPost("{userId}/setRole")]
        public async Task<ActionResult> SetRole(int userId, UserRoleDto dto)
        {
            await _userService.SetUserRole(userId, dto);

            return Ok();
        }

        /// <summary>
        /// Удалить роль у пользователя (SuperUser, AdminRegistration)
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="dto">Принимает UserRoleDto</param>
        /// <returns></returns>
        [Authorize(Roles = "SuperUser, AdminRegistration")]
        [HttpPost("{userId}/deleteRole")]
        public async Task<ActionResult> DeleteRole(int userId, UserRoleDto dto)
        {
            await _userService.DeleteUserRole(userId, dto);

            return Ok();
        }

        /// <summary>
        /// Получить список ролей пользователя (SuperUser, AdminRegistration)
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <returns>Возвращает массив UserRoleDto</returns>
        [Authorize(Roles = "SuperUser, AdminRegistration")]
        [HttpGet("{userId}/getRoles")]
        public async Task<ActionResult<List<UserRoleDto>>> GetRoles(int userId)
        {
            var result = await _userService.GetUserRoles(userId);

            return Ok(result);
        }

        /// <summary>
        /// Получить Id текущего пользователя
        /// </summary>
        /// <returns>Возвращает UserIdDto</returns>
        [Authorize]
        [HttpGet("getMyId")]
        public ActionResult<UserIdDto> GetCurrentUserId()
        {
            var token = Request.Cookies["choco-cookies"] ?? string.Empty;

            var result = _authService.GetUserIdByToken(token);

            return Ok(result);
        }
    }
}
