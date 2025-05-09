using Kventin.Services.Dtos.Filters;
using Kventin.Services.Dtos.Users;
using Kventin.Services.Infrastructure.Exceptions;
using Kventin.Services.Interfaces.Services;
using Kventin.Services.Services;
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
        /// Назначить роли пользователю (SuperUser, AdminRegistration)
        /// Пользователь с ролью AdminRegistration не может назначать роль SuperUser
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="dtos">Принимает массив UserRoleDto</param>
        /// <returns></returns>
        [Authorize(Roles = "SuperUser, AdminRegistration")]
        [HttpPost("{userId}/setRoles")]
        public async Task<ActionResult> SetRoles(int userId, List<UserRoleDto> dtos)
        {
            try
            {
                await _userService.SetUserRoles(userId, dtos);
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
        /// Удалить роли у пользователя (SuperUser, AdminRegistration)
        /// Пользователь с ролью AdminRegistration не может удалять роль SuperUser
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="dtos">Принимает массив UserRoleDto</param>
        /// <returns></returns>
        [Authorize(Roles = "SuperUser, AdminRegistration")]
        [HttpPost("{userId}/deleteRoles")]
        public async Task<ActionResult> DeleteRoles(int userId, List<UserRoleDto> dtos)
        {
            try
            {
                await _userService.DeleteUserRole(userId, dtos);
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
            var userIdDto = _authService.GetUserIdByCookie(Request.Cookies);

            var result = await _userService.GetUsersWithRoles(filter, userIdDto.UserId);

            return Ok(result);
        }

        /// <summary>
        /// Получить список ролей пользователя (SuperUser, AdminRegistration)
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <returns>Возвращает массив UserRoleDto</returns>
        [Authorize]
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
            var result = _authService.GetUserIdByCookie(Request.Cookies);

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

        /// <summary>
        /// Установить связь между родителем и ребенком
        /// (SuperUser, AdminRegistration)
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="childId"></param>
        /// <returns></returns>
        [Authorize(Roles = "SuperUser, AdminRegistration")]
        [HttpPost("{parentId}/addChild/{childId}")]
        public async Task<ActionResult> SetChildForParent(int parentId, int childId)
        {
            try
            {
                await _userService.SetChildForParent(parentId, childId);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Получить детей родителя
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Parent")]
        [HttpGet("{parentId}/getChildren")]
        public async Task<ActionResult<GetUsersChildrenDto>> GetChildren(int parentId)
        {
            var result = await _userService.GetUsersChildren(parentId);

            return Ok(result);
        }

        /// <summary>
        /// Выбрать ребенка. Чтобы отменить выбор ребенка childId должен быть равен 0
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="childId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Parent")]
        [HttpPost("{parentId}/selectChild/{childId}")]
        public async Task<ActionResult> SelectChild(int parentId, int childId)
        {
            try
            {
                var newToken = await _authService.GetNewCookieWithChildId(Request.Cookies, parentId, childId);

                Response.Cookies.Append("choco-cookies", newToken);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
