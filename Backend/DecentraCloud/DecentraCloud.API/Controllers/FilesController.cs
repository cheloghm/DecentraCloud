﻿using DecentraCloud.API.DTOs;
using DecentraCloud.API.Helpers;
using DecentraCloud.API.Interfaces.RepositoryInterfaces;
using DecentraCloud.API.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.StaticFiles;
using Swashbuckle.AspNetCore.Annotations;

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

        [HttpGet("all")]
        public async Task<IActionResult> GetAllFiles()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found." });
            }

            var files = await _fileService.GetAllFiles(userId);
            return Ok(files);
        }

        [HttpGet("view/{fileId}")]
        public async Task<IActionResult> ViewFile(string fileId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found." });
            }
            var fileRecord = await _fileService.GetFile(fileId);

            if (fileRecord == null || fileRecord.UserId != userId)
            {
                return NotFound();
            }

            var content = await _fileService.ViewFile(userId, fileId);
            if (content == null)
            {
                return BadRequest("Unable to view file.");
            }

            return File(content, fileRecord.MimeType, fileRecord.Filename);
        }

        [HttpGet("download/{fileId}")]
        public async Task<IActionResult> DownloadFile(string fileId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found." });
            }

            var fileDownloadDto = await _fileService.DownloadFile(userId, fileId);

            if (fileDownloadDto == null)
            {
                return NotFound(new { message = "File not found." });
            }

            // Return the decrypted content with the original filename
            return File(fileDownloadDto.Content, "application/octet-stream", fileDownloadDto.Filename);
        }

        [HttpGet("{fileId}")]
        public async Task<IActionResult> GetFile(string fileId)
        {
            var fileRecord = await _fileService.GetFile(fileId);
            if (fileRecord == null)
            {
                return NotFound(new { message = "File not found." });
            }

            return Ok(fileRecord);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchFiles([FromQuery] string query)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found." });
            }

            var files = await _fileService.SearchFiles(userId, query);
            return Ok(files);
        }

        [HttpDelete("delete/{fileId}")]
        [Authorize]
        public async Task<IActionResult> DeleteFile(string fileId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found." });
            }

            var result = await _fileService.DeleteFile(fileId, userId);
            if (!result)
            {
                return BadRequest(new { message = "Failed to delete file." });
            }

            return NoContent();
        }

        [HttpPost("share/{fileId}")]
        public async Task<IActionResult> ShareFile(string fileId, [FromBody] ShareFileDto shareFileDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found." });
            }

            var isOwner = await _fileService.IsFileOwner(userId, fileId);
            if (!isOwner)
            {
                return Forbid(); // Use Forbid() without arguments
            }

            var result = await _fileService.ShareFile(fileId, shareFileDto.Email);
            if (!result)
            {
                return BadRequest(new { message = "Failed to share file." });
            }

            return Ok(new { message = "File shared successfully." });
        }

        [HttpGet("shared-with-me")]
        public async Task<IActionResult> GetFilesSharedWithMe()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found." });
            }

            var files = await _fileService.GetFilesSharedWithUser(userId);
            return Ok(files);
        }

        [HttpGet("details/{fileId}")]
        public async Task<IActionResult> GetFileDetails(string fileId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                {
                    return Unauthorized(new { message = "User ID not found." });
                }

                var fileDetails = await _fileService.GetFileDetails(fileId, userId);
                return Ok(fileDetails);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
