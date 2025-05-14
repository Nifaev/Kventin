using Kventin.Services.Dtos.Files;
using Kventin.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Kventin.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/file")]
    public class FileController(IFileService fileService,
        IAuthService authService) : ControllerBase
    {
        private readonly IFileService _fileService = fileService;
        private readonly IAuthService _authService = authService;

        /// <summary>
        /// Загрузить файл
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("upload")]
        public async Task<ActionResult> UploadFile(IFormFile file)
        {
            var uploadedByUserId = _authService.GetUserIdByCookie(Request.Cookies);

            await _fileService.UploadFile(file, uploadedByUserId);

            return Ok();
        }

        /// <summary>
        /// Получить метаинформацию по ID
        /// </summary>
        [HttpGet("{fileId}")]
        public async Task<ActionResult<FileInfoDto>> GetFileInfo(int fileId)
        {
            var result = await _fileService.GetFileInfo(fileId);

            return Ok(result);
        }

        /// <summary>
        /// Скачать файл по ID
        /// </summary>
        [HttpGet("{fileId}/download")]
        public async Task<ActionResult> DownloadFile(int fileId)
        {
            var dto = await _fileService.DownloadFile(fileId);

            return File(dto.File, dto.ContentType, dto.OriginalFileName);
        }

        /// <summary>
        /// Удалить файл по ID
        /// </summary>
        [HttpDelete("{fileId}")]
        public async Task<ActionResult> DeleteFile(int fileId)
        {
            await _fileService.DeleteFile(fileId);

            return Ok();
        }
    }
}
