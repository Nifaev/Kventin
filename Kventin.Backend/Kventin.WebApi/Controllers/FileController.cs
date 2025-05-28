using Kventin.Services.Dtos.Files;
using Kventin.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kventin.WebApi.Controllers
{
    [ApiController]
    [Route("api/file")]
    public class FileController(IFileService fileService,
        IAuthService authService) : ControllerBase
    {
        private readonly IFileService _fileService = fileService;
        private readonly IAuthService _authService = authService;

        /// <summary>
        /// Загрузить файл - тестовый вариант на всякий случай (SuperUser)
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("upload")]
        [Authorize(Roles = "SuperUser")]
        public async Task<ActionResult> UploadFile(IFormFile file)
        {
            var uploadedByUserId = _authService.GetUserIdByCookie(Request.Cookies);

            await _fileService.UploadFileWithoutLinks(file, uploadedByUserId);

            return Ok();
        }

        /// <summary>
        /// Получить метаинформацию по ID - тестовый вариант на всякий случай (SuperUser)
        /// </summary>
        [HttpGet("{fileId}")]
        [Authorize(Roles = "SuperUser")]
        public async Task<ActionResult<FileInfoDto>> GetFileInfo(long fileId)
        {
            var result = await _fileService.GetFileInfo(fileId);

            return Ok(result);
        }

        /// <summary>
        /// Скачать файл по Id - использовать для скачивания любого файла (Все авторизованные пользователи)
        /// </summary>
        [HttpGet("{fileId}/download")]
        [Authorize]
        public async Task<ActionResult> DownloadFile(long fileId)
        {
            var dto = await _fileService.DownloadFile(fileId);

            return File(dto.File, dto.ContentType, dto.OriginalFileName);
        }

        /// <summary>
        /// Удалить файл по ID - тестовый вариант на всякий случай (SuperUser)
        /// </summary>
        [HttpDelete("{fileId}")]
        [Authorize(Roles = "SuperUser")]
        public async Task<ActionResult> DeleteFile(long fileId)
        {
            await _fileService.DeleteFiles([fileId]);

            return Ok();
        }
    }
}
