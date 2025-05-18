using Kventin.Services.Dtos.Lessons;
using Kventin.Services.Dtos.Marks;
using Kventin.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kventin.WebApi.Controllers
{
    [ApiController]
    [Route("api/lessons")]
    public class LessonController(ILessonService lessonService,
        IAuthService authService) : ControllerBase
    {
        private readonly ILessonService _lessonService = lessonService;
        private readonly IAuthService _authService = authService;

        /// <summary>
        /// Получить расписание на неделю (Все авторизованные пользователи)
        /// SuperUser и все админы получают расписание всего центра
        /// Student/Teacher получают расписание своих занятий
        /// Parent получает расписание занятий выбранного ребенка
        /// </summary>
        /// <param name="skipWeeksCount">0 - текущая неделя, -1 - прошлая неделя, 1 следующая неделя и тд</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("getSchoolWeek")]
        public async Task<ActionResult<SchoolWeekDto>> GetSchoolWeek(int skipWeeksCount)
        {
            var userId = _authService.GetUserIdByCookie(Request.Cookies);
            var userRoles = _authService.GetUserRolesByCookie(Request.Cookies);
            var childId = _authService.GetChildIdByCookie(Request.Cookies);

            try
            {
                var result = await _lessonService.GetSchoolWeek(skipWeeksCount, userId, userRoles, childId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Получить расписание на день (Все авторизованные пользователи)
        /// SuperUser и все админы получают расписание всего центра
        /// Student/Teacher получают расписание своих занятий
        /// Parent получает расписание занятий выбранного ребенка
        /// </summary>
        /// <param name="date">Дата занятий</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("getSchoolDay")]
        public async Task<ActionResult<SchoolDayDto>> GetSchoolDay(DateOnly date)
        {
            var userId = _authService.GetUserIdByCookie(Request.Cookies);
            var userRoles = _authService.GetUserRolesByCookie(Request.Cookies);
            var childId = _authService.GetChildIdByCookie(Request.Cookies);

            try
            {
                var result = await _lessonService.GetSchoolDay(date, userId, userRoles, childId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Получить всю информацию о занятии (Все авторизованные пользователи)
        /// SuperUser, Teacher и все админы также получают: всех учеников, их оценки за занятие, все задания
        /// Parent/Student помимо информации о занятии получают свои оценки за занятия, 
        /// свои задания (индивидуальные и групповые) и оценки за них
        /// </summary>
        /// <param name="lessonId">Id занятия</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{lessonId}")]
        public async Task<ActionResult<LessonFullInfoDto>> GetLessonFullInfo(int lessonId)
        {
            var userId = _authService.GetUserIdByCookie(Request.Cookies);
            var userRoles = _authService.GetUserRolesByCookie(Request.Cookies);
            var childId = _authService.GetChildIdByCookie(Request.Cookies);

            try
            {
                var result = await _lessonService.GetLessonFullInfo(lessonId, userId, userRoles, childId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Обновить информацию о занятии (Teacher, AdminLessons, SuperUser)
        /// </summary>
        /// <param name="lessonId">Id занятия</param>
        /// <param name="dto">UpdateLessonDto</param>
        /// <returns></returns>
        [Authorize(Roles = "Teacher, AdminLessons, SuperUser")]
        [HttpPost("{lessonId}/update")]
        public async Task<ActionResult> UpdateLesson(int lessonId, UpdateLessonDto dto)
        {
            try
            {
                await _lessonService.UpdateLesson(lessonId, dto);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Прикрепить файл к занятию (Teacher, AdminLessons, SuperUser)
        /// </summary>
        /// <param name="lessonId"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [Authorize(Roles = "Teacher, AdminLessons, SuperUser")]
        [HttpPost("{lessonId}/uploadFile")]
        public async Task<ActionResult> UploadFile(int lessonId, IFormFile file)
        {
            var userId = _authService.GetUserIdByCookie(Request.Cookies);

            try
            {
                await _lessonService.AttachFileToLesson(lessonId, file, userId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Удалить файл из занятия (Teacher, AdminLessons, SuperUser)
        /// </summary>
        /// <param name="lessonId">Id занятия</param>
        /// <param name="fileId">Id файла</param>
        /// <returns></returns>
        [Authorize(Roles = "Teacher, AdminLessons, SuperUser")]
        [HttpPost("{lessonId}/deleteFile/{fileId}")]
        public async Task<ActionResult> DeleteFile(int lessonId, int fileId)
        {
            try
            {
                await _lessonService.DetachFileFromLesson(lessonId, fileId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Отметить посещаемость занятия (Teacher, AdminLessons, SuperUser)
        /// </summary>
        /// <param name="lessonId"></param>
        /// <param name="attendedStudentIds"></param>
        /// <returns></returns>
        [Authorize(Roles = "Teacher, AdminLessons, SuperUser")]
        [HttpPost("{lessonId}/markAttendance")]
        public async Task<ActionResult> MarkAttendance(int lessonId, List<int> attendedStudentIds)
        {
            try
            {
                await _lessonService.MarkAttendance(lessonId, attendedStudentIds);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
