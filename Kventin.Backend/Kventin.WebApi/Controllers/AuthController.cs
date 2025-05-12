using Kventin.Services.Dtos.Auth;
using Kventin.Services.Infrastructure.Exceptions;
using Kventin.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kventin.WebApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        /// <summary>
        /// Регистрация
        /// </summary>
        /// <param name="dto">
        /// Принимает RegisterDto
        /// </param>
        /// <returns></returns>
        /// <response code="200">Успешная регистрация</response>
        /// <response code="400">Ошибка (см. сообщение)</response>
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto dto)
        {
            try
            {
                await _authService.Register(dto);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Авторизация
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <response code="200">Успешная авторизация</response>
        /// <response code="400">Ошибка (см. сообщение)</response>
        /// <response code="403">Доступ запрещён (не подтвержден аккаунт)</response>
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto dto)
        {
            try
            {
                var token = await _authService.Login(dto);

                Response.Cookies.Append("choco-cookies", token);

                return Ok();
            }
            catch (AuthException ex)
            {
                return StatusCode(403, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Выход из аккаунта
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        public ActionResult Logout()
        {
            Response.Cookies.Delete("choco-cookies");

            return Ok();
        }
    }
}
