using DecentraCloud.API.DTOs;
using DecentraCloud.API.Helpers;
using DecentraCloud.API.Interfaces.RepositoryInterfaces;
using DecentraCloud.API.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DecentraCloud.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IFileRepository _fileRepository;
        private readonly IUserRepository _userRepository;

        public FileController(IFileService fileService, IFileRepository fileRepository, IUserRepository userRepository)
        {
            _fileService = fileService;
            _fileRepository = fileRepository;
            _userRepository = userRepository;
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found." });
            }

            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "File data is missing." });
            }

            var fileUploadDto = new FileUploadDto
            {
                UserId = userId,
                Filename = file.FileName,  // Original filename
                Data = await FileHelper.ConvertToByteArrayAsync(file)
            };

            var result = await _fileService.UploadFile(fileUploadDto);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
