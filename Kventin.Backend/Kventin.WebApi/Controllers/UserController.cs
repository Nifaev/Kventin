using Kventin.Services.Dtos.Filters;
using Kventin.Services.Dtos.User;
using Kventin.Services.Infrastructure.Exceptions;
using Kventin.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kventin.WebApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController(IUserService userService,
        IAuthService authService) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly IAuthService _authService = authService;

        /// <summary>
        /// Назначить роль пользователю (SuperUser, AdminRegistration)
        /// Пользователь с ролью AdminRegistration не может назначать роль SuperUser
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="dto">Принимает UserRoleDto</param>
        /// <returns></returns>
        [Authorize(Roles = "SuperUser, AdminRegistration")]
        [HttpPost("{userId}/setRole")]
        public async Task<ActionResult> SetRole(int userId, UserRoleDto dto)
        {
            try
            {
                await _userService.SetUserRole(userId, dto);
            }
            catch (EntityNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (NoAccessException e)
            {
                return Forbid(e.Message);
            }

            return Ok();
        }

        /// <summary>
        /// Удалить роль у пользователя (SuperUser, AdminRegistration)
        /// Пользователь с ролью AdminRegistration не может удалять роль SuperUser
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="dto">Принимает UserRoleDto</param>
        /// <returns></returns>
        [Authorize(Roles = "SuperUser, AdminRegistration")]
        [HttpPost("{userId}/deleteRole")]
        public async Task<ActionResult> DeleteRole(int userId, UserRoleDto dto)
        {
            try
            {
                await _userService.DeleteUserRole(userId, dto);
            }
            catch (EntityNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (NoAccessException e)
            {
                return Forbid(e.Message);
            }

            return Ok();
        }

        /// <summary>
        /// Получить список всех пользователей (кроме себя) и их ролей (SuperUser, AdminRegistration).
        /// Пользователь с ролью AdminRegistration видит всех пользователей, кроме SuperUser.
        /// Пользователь с ролью SuperUser видит всех пользователей.
        /// </summary>
        /// <param name="filter">Принимает BaseFilterDto - параметры пагинации</param>
        /// <returns></returns>
        [Authorize(Roles = "SuperUser, AdminRegistration")]
        [HttpPost("getUsersRolesInfoPaged")]
        public async Task<ActionResult<List<UsersRolesInfoDto>>> GetUsersRolesInfo(BaseFilterDto filter)
        {
            var token = Request.Cookies["choco-cookies"] ?? string.Empty;

            var userIdDto = _authService.GetUserIdByToken(token);

            var result = await _userService.GetUsersWithRoles(filter, userIdDto.UserId);

            return Ok(result);
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
            List<UserRoleDto> result;
            
            try
            {
                result = await _userService.GetUserRoles(userId);
            }
            catch (EntityNotFoundException e)
            {
                return BadRequest(e.Message);
            }

            return Ok(result);
        }

        /// <summary>
        /// Получить Id текущего пользователя
        /// </summary>
        /// <returns>Возвращает UserIdDto</returns>
        //[Authorize]
        [HttpGet("getMyId")]
        public ActionResult<UserIdDto> GetCurrentUserId()
        {
            var token = Request.Cookies["choco-cookies"] ?? string.Empty;

            var result = _authService.GetUserIdByToken(token);

            return Ok(result);
        }

        /// <summary>
        /// Получить все возможные роли
        /// </summary>
        /// <returns>Массив UserRoleDto - Список всех ролей в системе</returns>
        //[Authorize(Roles = "SuperUser, AdminRegistration")]
        [HttpGet("getAllRoles")]
        public async Task<ActionResult<List<UserRoleDto>>> GetAllRoles()
        {
            var result = await _userService.GetAllRoles();

            return Ok(result);
        }
    }
}
