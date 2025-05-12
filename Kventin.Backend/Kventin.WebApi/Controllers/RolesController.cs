using Kventin.Services.Dtos.Users;
using Kventin.Services.Infrastructure.Exceptions;
using Kventin.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kventin.WebApi.Controllers
{
    [ApiController]
    [Route("api/roles")]
    public class RolesController(IRoleService roleService,
        IAuthService authService) : ControllerBase
    {
        private readonly IRoleService _roleService = roleService;
        private readonly IAuthService _authService = authService;

        /// <summary>
        /// Назначить роли пользователю (SuperUser, AdminRegistration)
        /// Пользователь с ролью AdminRegistration не может назначать 
        /// админские роли и SuperUser
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="rolenames">Принимает массив строк (названий ролей)</param>
        /// <returns></returns>
        /// <response code="200">Успешно</response>
        /// <response code="400">Ошибка (см. сообщение)</response>
        [Authorize(Roles = "SuperUser, AdminRegistration")]
        [HttpPost("{userId}/setRoles")]
        public async Task<ActionResult> SetRoles(int userId, List<string> rolenames)
        {
            try
            {
                var authorizedUserId = _authService.GetUserIdByCookie(Request.Cookies);

                await _roleService.SetUserRoles(authorizedUserId, userId, rolenames);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        /// <summary>
        /// Удалить роли у пользователя (SuperUser, AdminRegistration)
        /// Пользователь с ролью AdminRegistration не может удалять админские роли и SuperUser
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="rolenames">Принимает массив строк (названий ролей)</param>
        /// <returns></returns>
        /// <response code="200">Успешно</response>
        /// <response code="400">Ошибка (см. сообщение)</response>
        [Authorize(Roles = "SuperUser, AdminRegistration")]
        [HttpPost("{userId}/deleteRoles")]
        public async Task<ActionResult> DeleteRoles(int userId, List<string> rolenames)
        {
            try
            {
                var authorizedUserId = _authService.GetUserIdByCookie(Request.Cookies);

                await _roleService.DeleteUserRole(authorizedUserId, userId, rolenames);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        /// <summary>
        /// Получить список всех пользователей (кроме себя) и их ролей (SuperUser, AdminRegistration).
        /// Пользователь с ролью AdminRegistration видит всех пользователей, кроме админов и SuperUser.
        /// Пользователь с ролью SuperUser видит всех пользователей (кроме себя).
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Успешно</response>
        [Authorize(Roles = "SuperUser, AdminRegistration")]
        [HttpPost("getUsersRolesInfo")]
        public async Task<ActionResult<List<UserRoleInfoDto>>> GetAllUsersWithRoles()
        {
            var userId = _authService.GetUserIdByCookie(Request.Cookies);

            var result = await _roleService.GetAllUsersWithRoles(userId);

            return Ok(result);
        }

        /// <summary>
        /// Получить список ролей пользователя (SuperUser, AdminRegistration)
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <returns>Возвращает массив UserRoleDto</returns>
        /// <response code="200">Успешно</response>
        /// <response code="400">Ошибка (см. сообщение)</response>
        [Authorize(Roles = "SuperUser, AdminRegistration")]
        [HttpGet("{userId}/getRoles")]
        public async Task<ActionResult<List<string>>> GetRoles(int userId)
        {
            try
            {
                var result = await _roleService.GetUserRoles(userId);

                return Ok(result);
            }
            catch (EntityNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Получить список ролей текущего авторизованного пользователя
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("getMyRoles")]
        public ActionResult<List<string>> GetMyRoles()
        {
            var result = _authService.GetUserRolesByCookie(Request.Cookies);

            return Ok(result);
        }

        /// <summary>
        /// Получить все возможные роли (Все авторизованные пользователи)
        /// </summary>
        /// <returns>Массив UserRoleDto - Список всех ролей в системе</returns>
        [Authorize]
        [HttpGet("getAllRoles")]
        public async Task<ActionResult<List<string>>> GetAllRoles()
        {
            var result = await _roleService.GetAllRoles();

            return Ok(result);
        }
    }
}
