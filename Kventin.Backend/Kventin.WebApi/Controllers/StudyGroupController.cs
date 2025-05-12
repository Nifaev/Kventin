using Kventin.Services.Dtos.StudyGroups;
using Kventin.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kventin.WebApi.Controllers
{
    [ApiController]
    [Route("api/studyGroup")]
    public class StudyGroupController(IStudyGroupService studyGroupService) : ControllerBase
    {
        private readonly IStudyGroupService _studyGroupService = studyGroupService;

        /// <summary>
        /// Создать группу (SuperUser, AdminGroups)
        /// </summary>
        /// <param name="dto">Принимает CreateStudyGroupDto</param>
        /// <returns></returns>
        /// <response code="200">Успешно</response>
        /// <response code="400">Ошибка (см. сообщение)</response>
        [HttpPost("create")]
        [Authorize(Roles = "SuperUser, AdminGroups")]
        public async Task<ActionResult> CreateStudyGroup(CreateStudyGroupDto dto)
        {
            try
            {
                await _studyGroupService.CreateStudyGroup(dto);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Удалить группу по Id (SuperUser, AdminGroups)
        /// </summary>
        /// <param name="studyGroupId">Id группы</param>
        /// <returns></returns>
        /// <response code="200">Успешно</response>
        /// <response code="400">Ошибка (см. сообщение)</response>
        [HttpDelete("{studyGroupId}/delete")]
        [Authorize(Roles = "SuperUser, AdminGroups")]
        public async Task<ActionResult> DeleteStudyGroupById(int studyGroupId)
        {
            try
            {
                await _studyGroupService.DeleteStudyGroup(studyGroupId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Изменить группу (SuperUser, AdminGroups)
        /// </summary>
        /// <param name="studyGroupId">Id группы</param>
        /// <param name="dto">Принимает UpdateStudyGroupDto</param>
        /// <returns></returns>
        /// <response code="200">Успешно</response>
        /// <response code="400">Ошибка (см. сообщение)</response>
        [HttpPost("{studyGroupId}/update")]
        [Authorize(Roles = "SuperUser, AdminGroups")]
        public async Task<ActionResult> UpdateStudyGroupById(int studyGroupId, UpdateStudyGroupDto dto)
        {
            try
            {
                await _studyGroupService.UpdateStudyGroup(studyGroupId, dto);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Получить полную информацию о группе (Все авторизованные пользователи)
        /// </summary>
        /// <param name="studyGroupId">Id группы</param>
        /// <returns></returns>
        /// <response code="200">Успешно</response>
        /// <response code="400">Ошибка (см. сообщение)</response>
        [HttpGet("{studyGroupId}")]
        [Authorize]
        public async Task<ActionResult<StudyGroupFullInfoDto>> GetStudyGroupInfoById(int studyGroupId)
        {
            try
            {
                var result = await _studyGroupService.GetStudyGroupFullInfo(studyGroupId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Получить список всех групп (SuperUser, AdminGroups)
        /// </summary>
        /// <returns>Возвращает массив StudyGroupShortInfoDto</returns>
        /// <response code="200">Успешно</response>
        /// <response code="400">Ошибка (см. сообщение)</response>
        [HttpGet("all")]
        [Authorize(Roles = "SuperUser, AdminGroups")]
        public async Task<ActionResult<List<StudyGroupShortInfoDto>>> GetAllStudyGroupsShortInfo()
        {
            try
            {
                var result = await _studyGroupService.GetAllStudyGroupsShortInfo();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Добавить ученика в группу
        /// </summary>
        /// <param name="studyGroupId">Id группы</param>
        /// <param name="studentId">Id ученика</param>
        /// <returns></returns>
        /// <response code="200">Успешно</response>
        /// <response code="400">Ошибка (см. сообщение)</response>
        [HttpPost("{studyGroupId}/addStudent")]
        [Authorize(Roles = "SuperUser, AdminGroups")]
        public async Task<ActionResult> AddStudentToStudyGroup(int studyGroupId, int studentId)
        {
            try
            {
                await _studyGroupService.AddStudentToStudyGroup(studyGroupId, studentId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Удалить ученик из группы
        /// </summary>
        /// <param name="studyGroupId">Id группы</param>
        /// <param name="studentId">Id ученика</param>
        /// <returns></returns>
        /// <response code="200">Успешно</response>
        /// <response code="400">Ошибка (см. сообщение)</response>
        [HttpDelete("{studyGroupId}/deleteStudent")]
        [Authorize(Roles = "SuperUser, AdminGroups")]
        public async Task<ActionResult> DeleteStudentFromStudyGroup(int studyGroupId, int studentId)
        {
            try
            {
                await _studyGroupService.DeleteStudentFromStudyGroup(studyGroupId, studentId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
