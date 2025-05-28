using Kventin.Services.Dtos.Users;
using Kventin.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

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
        /// Получить Id текущего авторизованного пользователя (Все авторизованные пользователи)
        /// </summary>
        /// <returns>Возвращает UserIdDto</returns>
        [Authorize]
        [HttpGet("getMyId")]
        public ActionResult<long> GetCurrentUserId()
        {
            var result = _authService.GetUserIdByCookie(Request.Cookies);

            return Ok(result);
        }

        /// <summary>
        /// Установить связь между родителем и детьми
        /// (SuperUser, AdminRegistration)
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="childrenIds">Принимает массив int - массив Id - шников учеников</param>
        /// <returns></returns>
        /// <response code="200">Успешно</response>
        /// <response code="400">Ошибка (см. сообщение)</response>
        [Authorize(Roles = "SuperUser, AdminRegistration")]
        [HttpPost("{parentId}/addChildren")]
        public async Task<ActionResult> SetChildrenForParent(long parentId, List<long> childrenIds)
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
        public async Task<ActionResult<List<UserShortInfoDto>>> GetChildren(long parentId)
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
        /// <response code="200">Успешно</response>
        /// <response code="400">Ошибка (см. сообщение)</response>
        [Authorize(Roles = "Parent")]
        [HttpPost("{parentId}/selectChild/{childId}")]
        public async Task<ActionResult> SelectChild(long parentId, long childId)
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
