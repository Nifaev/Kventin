using Azure.Core;
using Kventin.Services.Dtos.Marks;
using Kventin.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kventin.WebApi.Controllers
{
    [ApiController]
    [Route("api/mark")]
    public class MarkController(IMarkService markService,
        IAuthService authService) : ControllerBase
    {
        private readonly IMarkService _markService = markService;
        private readonly IAuthService _authService = authService;

        /// <summary>
        /// Поставить ученикам оценки за занятие (Teacher, AdminLessons, SuperUser).
        /// Можно одновременно поставить оценки нескольким ученикам,
        /// а также ставить несколько оценок каждому ученику
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize(Roles = "Teacher, AdminLessons, SuperUser")]
        [HttpPost("assignMarksForLesson")]
        public async Task<ActionResult> AssignMarksForLesson(AssignMarksForLessonDto dto)
        {
            var userId = _authService.GetUserIdByCookie(Request.Cookies);

            try
            {
                await _markService.AssignMarksForLesson(userId, dto);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Поставить ученикам оценки за задание (Teacher, AdminLessons, SuperUser).
        /// Можно одновременно поставить оценки нескольким ученикам,
        /// а также ставить несколько оценок каждому ученику
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize(Roles = "Teacher, AdminLessons, SuperUser")]
        [HttpPost("assignMarkForExercise")]
        public async Task<ActionResult> AssignMarksForExercise(AssignMarksForExerciseDto dto)
        {
            var userId = _authService.GetUserIdByCookie(Request.Cookies);

            try
            {
                await _markService.AssignMarksForExercise(userId, dto);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Изменить оценку за занятие или задание (Teacher, AdminLessons, SuperUser).
        /// </summary>
        /// <param name="markId">Id оценки</param>
        /// <param name="dto">MarkShortInfoDto</param>
        /// <returns></returns>
        [Authorize(Roles = "Teacher, AdminLessons, SuperUser")]
        [HttpPost("{markId}/update")]
        public async Task<ActionResult> UpdateMarkForLesson(long markId, MarkShortInfoDto dto)
        {
            try
            {
                await _markService.UpdateMark(markId, dto);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Удалить оценку за занятие или за задание (Teacher, AdminLessons, SuperUser).
        /// </summary>
        /// <param name="markId">Id оценки</param>
        /// <returns></returns>
        [Authorize(Roles = "Teacher, AdminLessons, SuperUser")]
        [HttpDelete("{markId}/delete")]
        public async Task<ActionResult> DeleteMarkForLesson(long markId)
        {
            try
            {
                await _markService.DeleteMark(markId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
