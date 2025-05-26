using Kventin.Services.Dtos.Exercises;
using Kventin.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kventin.WebApi.Controllers
{
    [ApiController]
    [Route("api/exercise")]
    public class ExerciseController(IAuthService authService,
        IExerciseService exerciseService) : ControllerBase
    {
        private readonly IExerciseService _exerciseService = exerciseService;
        private readonly IAuthService _authService = authService;

        /// <summary>
        /// Обновить задание кроме группы и занятия (SuperUser, AdminLessons, Teacher)
        /// </summary>
        /// <param name="exerciseId"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize(Roles = "SuperUser, AdminLessons, Teacher")]
        [HttpPost("{exerciseId}/update")]
        public async Task<ActionResult> UpdateExercise(int exerciseId, UpdateExerciseDto dto)
        {
            try
            {
                await _exerciseService.UpdateExercise(exerciseId, dto);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Удалить задание (SuperUser, AdminLessons, Teacher)
        /// </summary>
        /// <param name="exerciseId"></param>
        /// <returns></returns>
        [Authorize(Roles = "SuperUser, AdminLessons, Teacher")]
        [HttpDelete("{exerciseId}/delete")]
        public async Task<ActionResult> DeleteExercise(int exerciseId)
        {
            try
            {
                await _exerciseService.DeleteExercise(exerciseId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Добавить задание (SuperUser, AdminLessons, Teacher)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize(Roles = "SuperUser, AdminLessons, Teacher")]
        [HttpPost("create")]
        public async Task<ActionResult> CreateExercise(CreateExerciseDto dto)
        {
            var teacherId = _authService.GetUserIdByCookie(Request.Cookies);

            try
            {
                await _exerciseService.CreateExercise(teacherId, dto);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Получить полную информацию о задании (Все авторизованные пользователи).
        /// </summary>
        /// <param name="exerciseId">Id задания</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{exerciseId}")]
        public async Task<ActionResult<ExerciseFullInfoDto>> GetExerciseFullInfo(int exerciseId)
        {
            var userId = _authService.GetUserIdByCookie(Request.Cookies);
            var userRoles = _authService.GetUserRolesByCookie(Request.Cookies);
            var childId = _authService.GetChildIdByCookie(Request.Cookies);

            try
            {
                var result = await _exerciseService.GetExerciseFullInfo(exerciseId, userId, userRoles, childId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Прикрепить файл к заданию (SuperUser, AdminLessons, Teacher)
        /// </summary>
        /// <param name="exerciseId">Id задания</param>
        /// <param name="files">Файлы</param>
        /// <returns></returns>
        [Authorize(Roles = "SuperUser, AdminLessons, Teacher")]
        [HttpPost("{exerciseId}/uploadFiles")]
        public async Task<ActionResult<ExerciseFullInfoDto>> UploadFilesToExercise(int exerciseId, List<IFormFile> files)
        {
            var userId = _authService.GetUserIdByCookie(Request.Cookies);

            try
            {
                await _exerciseService.AttachFilesToExercise(exerciseId, userId, files);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Удалить файл из задания (SuperUser, AdminLessons, Teacher)
        /// </summary>
        /// <param name="exerciseId">Id задания</param>
        /// <param name="fileIds">Id файлов</param>
        /// <returns></returns>
        [Authorize(Roles = "SuperUser, AdminLessons, Teacher")]
        [HttpDelete("{exerciseId}/deleteFiles")]
        public async Task<ActionResult<ExerciseFullInfoDto>> DeleteFileFromExercise(int exerciseId, List<int> fileIds)
        {
            try
            {
                await _exerciseService.DetachFilesFromExercise(exerciseId, fileIds);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
