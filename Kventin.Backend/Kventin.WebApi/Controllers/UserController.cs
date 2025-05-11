using Kventin.Services.Dtos.Users;
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
        /// Назначить роли пользователю (SuperUser, AdminRegistration)
        /// Пользователь с ролью AdminRegistration не может назначать 
        /// админские роли и SuperUser
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="rolenames">Принимает массив строк (названий ролей)</param>
        /// <returns></returns>
        [Authorize(Roles = "SuperUser, AdminRegistration")]
        [HttpPost("{userId}/setRoles")]
        public async Task<ActionResult> SetRoles(int userId, List<string> rolenames)
        {
            try
            {
                var authorizedUserId = _authService.GetUserIdByCookie(Request.Cookies);

                await _userService.SetUserRoles(authorizedUserId, userId, rolenames);
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
        [Authorize(Roles = "SuperUser, AdminRegistration")]
        [HttpPost("{userId}/deleteRoles")]
        public async Task<ActionResult> DeleteRoles(int userId, List<string> rolenames)
        {
            try
            {
                var authorizedUserId = _authService.GetUserIdByCookie(Request.Cookies);

                await _userService.DeleteUserRole(authorizedUserId, userId, rolenames);
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
        [Authorize(Roles = "SuperUser, AdminRegistration")]
        [HttpPost("getUsersRolesInfo")]
        public async Task<ActionResult<List<UserRoleInfoDto>>> GetAllUsersWithRoles()
        {
            var userId = _authService.GetUserIdByCookie(Request.Cookies);

            var result = await _userService.GetAllUsersWithRoles(userId);

            return Ok(result);
        }

        /// <summary>
        /// Получить список ролей пользователя (SuperUser, AdminRegistration)
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <returns>Возвращает массив UserRoleDto</returns>
        [Authorize(Roles = "SuperUser, AdminRegistration")]
        [HttpGet("{userId}/getRoles")]
        public async Task<ActionResult<List<string>>> GetRoles(int userId)
        {
            try
            {
                var result = await _userService.GetUserRoles(userId);

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
        /// Получить Id текущего авторизованного пользователя (Все авторизованные пользователи)
        /// </summary>
        /// <returns>Возвращает UserIdDto</returns>
        [Authorize]
        [HttpGet("getMyId")]
        public ActionResult<int> GetCurrentUserId()
        {
            var result = _authService.GetUserIdByCookie(Request.Cookies);

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
            var result = await _userService.GetAllRoles();

            return Ok(result);
        }

        /// <summary>
        /// Установить связь между родителем и детьми
        /// (SuperUser, AdminRegistration)
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="childrenIds">Принимает массив int - массив Id - шников учеников</param>
        /// <returns></returns>
        [Authorize(Roles = "SuperUser, AdminRegistration")]
        [HttpPost("{parentId}/addChildren")]
        public async Task<ActionResult> SetChildrenForParent(int parentId, List<int> childrenIds)
        {
            try
            {
                await _userService.SetChildrenForParent(parentId, childrenIds);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Получить детей родителя (Parent)
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns>Возвращает массив UserShortInfoDto</returns>
        [Authorize(Roles = "Parent")]
        [HttpGet("{parentId}/getChildren")]
        public async Task<ActionResult<List<UserShortInfoDto>>> GetChildren(int parentId)
        {
            var result = await _userService.GetUsersChildren(parentId);

            return Ok(result);
        }

        /// <summary>
        /// Выбрать ребенка. Чтобы отменить выбор ребенка передать childId = 0 (Parent)
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

        /// <summary>
        /// Получить список всех учеников (SuperUser, AdminRegistration)
        /// </summary>
        /// <returns>Возвращает массив UserShortInfoDto</returns>
        [Authorize(Roles = "SuperUser, AdminRegistration")]
        [HttpGet("getAllStudents")]
        public async Task<ActionResult<List<UserShortInfoDto>>> GetAllStudentsShortInfo()
        {
            var result = await _userService.GetAllUsersShortInfoByRole("Student");

            return Ok(result);
        }

        /// <summary>
        /// Получить список всех учителей (SuperUser, AdminRegistration)
        /// </summary>
        /// <returns>Возвращает массив UserShortInfoDto</returns>
        [Authorize(Roles = "SuperUser, AdminRegistration")]
        [HttpGet("getAllTeachers")]
        public async Task<ActionResult<List<UserShortInfoDto>>> GetAllTeachersShortInfo()
        {
            var result = await _userService.GetAllUsersShortInfoByRole("Teacher");

            return Ok(result);
        }

        /// <summary>
        /// Получить список всех родителей (SuperUser, AdminRegistration)
        /// </summary>
        /// <returns>Возвращает массив UserShortInfoDto</returns>
        [Authorize(Roles = "SuperUser, AdminRegistration")]
        [HttpGet("getAllParents")]
        public async Task<ActionResult<List<UserShortInfoDto>>> GetAllParentsShortInfo()
        {
            var result = await _userService.GetAllUsersShortInfoByRole("Parent");

            return Ok(result);
        }
    }
}
