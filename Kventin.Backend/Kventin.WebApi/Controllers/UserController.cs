using Kventin.Services.Dtos.User;
using Kventin.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kventin.WebApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterUserDto dto)
        {
            await _userService.Register(dto);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginUserDto dto)
        {
            var token = await _userService.Login(dto);

            Response.Cookies.Append("choco-cookies", token);

            return Ok();
        }

        [HttpPost("logout")]
        public ActionResult Logout()
        {
            Response.Cookies.Delete("choco-cookies");

            return Ok();
        }
    }
}
