using Kventin.Services.Dtos;
using Kventin.Services.Infrastructure.Exceptions;
using Kventin.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kventin.WebApi.Controllers
{
    [ApiController]
    [Route("api/subject")]
    public class SubjectController(ISubjectService subjectService) : ControllerBase
    {
        private readonly ISubjectService _subjectService = subjectService;

        [HttpPost("create")]
        public async Task<ActionResult> CreateSubject(SubjectDto dto)
        {
            await _subjectService.CreateSubject(dto);

            return Ok();
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<SubjectDto>>> GetAllSubjects()
        {
            return await _subjectService.GetAllSubjects();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubjectDto>> GetSubjectById(int id)
        {
            var result = await _subjectService.GetSubjectByid(id);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSubjectById(int id)
        {
            await _subjectService.DeleteSubjectById(id);

            return Ok();
        }
    }
}
