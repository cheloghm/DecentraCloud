using DecentraCloud.API.DTOs;
using DecentraCloud.API.Helpers;
using DecentraCloud.API.Interfaces.RepositoryInterfaces;
using DecentraCloud.API.Interfaces.ServiceInterfaces;
using DecentraCloud.API.Models;
using Microsoft.AspNetCore.Authorization;
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
                Filename = file.FileName,
                OriginalFilename = file.FileName,
                Data = await FileHelper.ConvertToByteArrayAsync(file)
            };

            var result = await _fileService.UploadFile(fileUploadDto);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpDelete("delete/{filename}")]
        public async Task<IActionResult> DeleteFile(string filename)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found." });
            }

            var fileOperationDto = new FileOperationDto
            {
                UserId = userId,
                Filename = filename
            };

            var result = await _fileService.DeleteFile(fileOperationDto);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("view/{filename}")]
        public async Task<IActionResult> ViewFile(string filename)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var fileRecord = await _fileRepository.GetFileRecordByOriginalFilename(userId, filename);

            if (fileRecord == null)
            {
                var sharedFile = await _fileRepository.GetFileByFilename(filename);
                if (sharedFile != null && await _fileService.HasPermission(userId, sharedFile.Id, "view"))
                {
                    fileRecord = sharedFile;
                }
                else
                {
                    return NotFound(new { message = "File not found or no permission" });
                }
            }

            var fileOperationDto = new FileOperationDto
            {
                UserId = userId,
                Filename = fileRecord.Filename, // Use the encrypted filename
                NodeId = fileRecord.NodeId
            };

            var result = await _fileService.ViewFile(fileOperationDto);

            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

        [HttpGet("download/{filename}")]
        public async Task<IActionResult> DownloadFile(string filename)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var fileRecord = await _fileRepository.GetFileRecordByOriginalFilename(userId, filename);

            if (fileRecord == null)
            {
                var sharedFile = await _fileRepository.GetFileByFilename(filename);
                if (sharedFile != null && await _fileService.HasPermission(userId, sharedFile.Id, "view"))
                {
                    fileRecord = sharedFile;
                }
                else
                {
                    return NotFound(new { message = "File not found or no permission" });
                }
            }

            var fileOperationDto = new FileOperationDto
            {
                UserId = userId,
                Filename = fileRecord.Filename, // Use the encrypted filename
                NodeId = fileRecord.NodeId
            };

            var result = await _fileService.DownloadFile(fileOperationDto);

            if (result != null)
            {
                return File(result.Content, "application/octet-stream", result.Filename);
            }

            return NotFound();
        }

        [HttpGet("files")]
        public async Task<IActionResult> GetFiles()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found." });
            }

            var files = await _fileRepository.GetFilesByUserId(userId);

            return Ok(files);
        }

        [HttpPost("share")]
        public async Task<IActionResult> ShareFile([FromBody] FileShareDto fileShareDto)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            fileShareDto.OwnerId = ownerId;

            var result = await _fileService.ShareFile(fileShareDto);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> RevokeFilePermission([FromBody] FileRevokePermissionDto fileRevokePermissionDto)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            fileRevokePermissionDto.OwnerId = ownerId;

            var result = await _fileService.RevokeFilePermission(fileRevokePermissionDto);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("permissions/{fileId}")]
        public async Task<IActionResult> GetFilePermissions(string fileId)
        {
            var permissions = await _fileService.GetFilePermissions(fileId);
            return Ok(permissions);
        }
    }
}
