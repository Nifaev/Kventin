using Kventin.Services.Dtos.Subjects;
using Kventin.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kventin.WebApi.Controllers
{
    [ApiController]
    [Route("api/subject")]
    public class SubjectController(ISubjectService subjectService,
        IAuthService authService) : ControllerBase
    {
        private readonly ISubjectService _subjectService = subjectService;
        private readonly IAuthService _authService = authService;

        /// <summary>
        /// Создать предмет (SuperUser, AdminSchedule)
        /// </summary>
        /// <param name="subjectName">Название предмета</param>
        /// <returns></returns>
        /// <response code="200">Успешно</response>
        /// <response code="400">Ошибка (см. сообщение)</response>
        [Authorize(Roles = "SuperUser, AdminSchedule")]
        [HttpPost("create")]
        public async Task<ActionResult> CreateSubject(string subjectName)
        {
            try
            {
                await _subjectService.CreateSubject(subjectName);

                return Ok();
            }
            catch (Exception ex)
            { 
                return BadRequest(ex.Message);  
            }
        }

        /// <summary>
        /// Получить все предметы (Все авторизованные пользователи)
        /// SuperUser и AdminSchedule получают список всех предметов
        /// Teacher/Student получают список своих предметов
        /// Parent получае тсписок предметов выбранного ребенка
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [Authorize]
        public async Task<ActionResult<List<SubjectDto>>> GetAllSubjects()
        {
            var userId = _authService.GetUserIdByCookie(Request.Cookies);
            var userRoles = _authService.GetUserRolesByCookie(Request.Cookies);
            var childId = _authService.GetChildIdByCookie(Request.Cookies);

            try
            {
                var result = await _subjectService.GetAllSubjects(userId, userRoles, childId);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Получить предмет по Id (SuperUser, AdminSchedule)
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        /// <response code="200">Успешно</response>
        /// <response code="400">Ошибка (см. сообщение)</response>
        [HttpGet("{subjectId}")]
        [Authorize(Roles = "SuperUser, AdminSchedule")]
        public async Task<ActionResult<SubjectDto>> GetSubjectById(int subjectId)
        {
            try
            {
                var result = await _subjectService.GetSubjectByid(subjectId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Удалить предмет по Id (SuperUser, AdminSchedule)
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        /// <response code="200">Успешно</response>
        /// <response code="400">Ошибка (см. сообщение)</response>
        [HttpDelete("{subjectId}/delete")]
        [Authorize(Roles = "SuperUser, AdminSchedule")]
        public async Task<ActionResult> DeleteSubjectById(int subjectId)
        {
            try
            {
                await _subjectService.DeleteSubjectById(subjectId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Изменить предмет по Id (SuperUser, AdminSchedule)
        /// </summary>
        /// <param name="subjectId"></param>
        /// <param name="newSubjectName"></param>
        /// <returns></returns>
        /// <response code="200">Успешно</response>
        /// <response code="400">Ошибка (см. сообщение)</response>
        [HttpPost("{subjectId}/update")]
        [Authorize(Roles = "SuperUser, AdminSchedule")]
        public async Task<ActionResult> UpdateSubjectById(int subjectId, string newSubjectName)
        {
            try 
            {
                await _subjectService.UpdateSubjectById(subjectId, newSubjectName);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
