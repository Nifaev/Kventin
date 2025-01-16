using Kventin.Services.Dtos;
using Kventin.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kventin.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirstController : ControllerBase
    {
        private readonly IFirstService _firstService;

        public FirstController(IFirstService firstService)
        {
            _firstService = firstService;
        }

        [HttpGet]
        public async Task<ActionResult<GetSubjectsDto>> Get()
        {
            var result = await _firstService.GetSubjects();

            return Ok(result);
        }
    }
}
