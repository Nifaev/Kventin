using Kventin.Services.Dtos.Auth;
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
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto dto)
        {
            await _authService.Register(dto);

            return Ok();
        }

        /// <summary>
        /// Авторизация
        /// </summary>
        /// <param name="dto">Принимает LoginDto</param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDto dto)
        {
            try
            {
                var token = await _authService.Login(dto);

                Response.Cookies.Append("choco-cookies", token);

                return Ok();
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
