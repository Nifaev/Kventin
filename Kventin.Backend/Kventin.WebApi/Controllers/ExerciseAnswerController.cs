using Kventin.Services.Dtos.ExerciseAnswers;
using Kventin.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kventin.WebApi.Controllers
{
    [ApiController]
    [Route("api/exerciseAnswer")]
    public class ExerciseAnswerController(IExerciseAnswerService answerService,
        IAuthService authService) : ControllerBase
    {
        private readonly IExerciseAnswerService _answerService = answerService;
        private readonly IAuthService _authService = authService;

        /// <summary>
        /// Добавить ответ на задание (Student)
        /// </summary>
        /// <param name="dto">CreateExerciseAnswerDto</param>
        /// <returns></returns>
        [Authorize("Student")]
        [HttpPost("create")]
        public async Task<ActionResult> CreateExerciseAnswer(CreateExerciseAnswerDto dto)
        {
            var studentId = _authService.GetUserIdByCookie(Request.Cookies);

            try
            {
                await _answerService.CreateExerciseAsnwer(studentId, dto);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Удалить ответ на задание (Student)
        /// </summary>
        /// <param name="exerciseAnswerId">Id ответа на задание</param>
        /// <returns></returns>
        [Authorize("Student")]
        [HttpDelete("{exerciseAnswerId}/delete")]
        public async Task<ActionResult> DeleteExerciseAnswer(long exerciseAnswerId)
        {
            var studentId = _authService.GetUserIdByCookie(Request.Cookies);

            try
            {
                await _answerService.DeleteExerciseAnswer(studentId, exerciseAnswerId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Изменить ответ на задание (Student)
        /// </summary>
        /// <param name="exerciseAnswerId">Id ответа на задание</param>
        /// <param name="answerContent">Содержание ответа</param>
        /// <returns></returns>
        [Authorize("Student")]
        [HttpPost("{exerciseAnswerId}/update")]
        public async Task<ActionResult> UpdateExerciseAnswer(long exerciseAnswerId, string answerContent)
        {
            var studentId = _authService.GetUserIdByCookie(Request.Cookies);

            try
            {
                await _answerService.UpdateExerciseAnswer(studentId, exerciseAnswerId, answerContent);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Добавить файлы к ответу на задание (Student)
        /// </summary>
        /// <param name="exerciseAnswerId">Id ответа на задание</param>
        /// <param name="files">Файлы</param>
        /// <returns></returns>
        [Authorize("Student")]
        [HttpPost("{exerciseAnswerId}/uploadFiles")]
        public async Task<ActionResult> UploadFilesToExerciseAnswer(long exerciseAnswerId, List<IFormFile> files)
        {
            var studentId = _authService.GetUserIdByCookie(Request.Cookies);

            try
            {
                await _answerService.AttachFilesToExerciseAnswer(studentId, exerciseAnswerId, files);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Удалить файлы из ответа на задание
        /// </summary>
        /// <param name="exerciseAnswerId">Id ответа на задание</param>
        /// <param name="fileIds">Id файлов, которые надо удалить</param>
        /// <returns></returns>
        [Authorize("Student")]
        [HttpDelete("{exerciseAnswerId}/deleteFile")]
        public async Task<ActionResult> DeleteFileFromExerciseAnswer(long exerciseAnswerId, List<long> fileIds)
        {
            var studentId = _authService.GetUserIdByCookie(Request.Cookies);

            try
            {
                await _answerService.DetachFilesFromExerciseAnswer(studentId, exerciseAnswerId, fileIds);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
