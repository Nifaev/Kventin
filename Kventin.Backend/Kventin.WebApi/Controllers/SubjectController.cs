using Kventin.Services.Dtos.Subjects;
using Kventin.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kventin.WebApi.Controllers
{
    [ApiController]
    [Route("api/subject")]
    public class SubjectController(ISubjectService subjectService) : ControllerBase
    {
        private readonly ISubjectService _subjectService = subjectService;

        /// <summary>
        /// Создать предмет (SuperUser, AdminSchedule)
        /// </summary>
        /// <param name="subjectName">Название предмета</param>
        /// <returns></returns>
        [Authorize(Roles = "SuperUser, AdminSchedule")]
        [HttpPost("create")]
        public async Task<ActionResult> CreateSubject(string subjectName)
        {
            await _subjectService.CreateSubject(subjectName);

            return Ok();
        }

        /// <summary>
        /// Получить все предметы (Все авторизованные пользователи)
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [Authorize]
        public async Task<ActionResult<List<SubjectDto>>> GetAllSubjects()
        {
            return await _subjectService.GetAllSubjects();
        }

        /// <summary>
        /// Получить предмет по Id (SuperUser, AdminSchedule)
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        [HttpGet("{subjectId}")]
        [Authorize(Roles = "SuperUser, AdminSchedule")]
        public async Task<ActionResult<SubjectDto>> GetSubjectById(int subjectId)
        {
            var result = await _subjectService.GetSubjectByid(subjectId);

            return Ok(result);
        }

        /// <summary>
        /// Удалить предмет по Id (SuperUser, AdminSchedule)
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        [HttpDelete("{subjectId}/delete")]
        [Authorize(Roles = "SuperUser, AdminSchedule")]
        public async Task<ActionResult> DeleteSubjectById(int subjectId)
        {
            await _subjectService.DeleteSubjectById(subjectId);

            return Ok();
        }

        /// <summary>
        /// Изменить предмет по Id (SuperUser, AdminSchedule)
        /// </summary>
        /// <param name="subjectId"></param>
        /// <param name="newSubjectName"></param>
        /// <returns></returns>
        [HttpPost("{subjectId}/update")]
        [Authorize(Roles = "SuperUser, AdminSchedule")]
        public async Task<ActionResult> UpdateSubjectById(int subjectId, string newSubjectName)
        {
            await _subjectService.UpdateSubjectById(subjectId, newSubjectName);

            return Ok();
        }
    }
}
