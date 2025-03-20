using Kventin.Services.Dtos.Lessons;
using Kventin.Services.Infrastructure.Exceptions;
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

        [Authorize(Roles = "Teacher, Superuser")]
        [HttpGet]
        [Route("/{lessonId}/teacher")]
        public async Task<ActionResult<GetLessonInfoForTeacherDto>> GetTeacherLessonInfo(int lessonId)
        {
            var userIdDto = _authService.GetUserIdByCookie(Request.Cookies);
            
            try
            {
                var result = await _lessonService.GetTeacherLessonInfo(lessonId, userIdDto.UserId);

                return Ok(result);
            }
            catch (EntityNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NoAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }

        [Authorize(Roles = "Student, Parent")]
        [Route("/{lessonId}/studentOrParent")]
        [HttpGet]
        public async Task<ActionResult<GetLessonInfoForStudentOrParentDto>> GetStudentOrParentLessonInfo(int lessonId)
        {
            var userIdDto = _authService.GetUserIdByCookie(Request.Cookies);

            try
            {
                var result = await _lessonService.GetStudentOrParentLessonInfo(lessonId, userIdDto.UserId);
                return Ok(result);
            }
            catch (EntityNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NoAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }
    }
}
