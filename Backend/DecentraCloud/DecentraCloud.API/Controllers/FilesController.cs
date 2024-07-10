using DecentraCloud.API.DTOs;
using DecentraCloud.API.Interfaces.RepositoryInterfaces;
using DecentraCloud.API.Interfaces.ServiceInterfaces;
using DecentraCloud.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DecentraCloud.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IFileRepository _fileRepository;

        public FileController(IFileService fileService, IFileRepository fileRepository)
        {
            _fileService = fileService;
            _fileRepository = fileRepository;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] FileUploadDto fileUploadDto)
        {
            var result = await _fileService.UploadFile(fileUploadDto);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteFile([FromBody] FileOperationDto fileOperationDto)
        {
            var fileRecord = await _fileRepository.GetFileRecord(fileOperationDto.UserId, fileOperationDto.Filename);
            if (fileRecord == null)
            {
                return NotFound();
            }

            fileOperationDto.NodeId = fileRecord.NodeId;
            var result = await _fileService.DeleteFile(fileOperationDto);

            if (result.Success)
            {
                await _fileRepository.DeleteFileRecord(fileOperationDto.UserId, fileOperationDto.Filename);
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("view/{filename}")]
        public async Task<IActionResult> ViewFile(string filename)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var fileRecord = await _fileRepository.GetFileRecord(userId, filename);
            if (fileRecord == null)
            {
                return NotFound();
            }

            var result = await _fileService.ViewFile(new FileOperationDto { UserId = userId, Filename = filename, NodeId = fileRecord.NodeId });

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
            var fileRecord = await _fileRepository.GetFileRecord(userId, filename);
            if (fileRecord == null)
            {
                return NotFound();
            }

            var result = await _fileService.DownloadFile(new FileOperationDto { UserId = userId, Filename = filename, NodeId = fileRecord.NodeId });

            if (result != null)
            {
                return File(result.Content, "application/octet-stream", result.Filename);
            }

            return NotFound();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchFiles([FromQuery] string query)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var results = await _fileRepository.SearchFileRecords(userId, query);
            return Ok(results);
        }

        [HttpPost("rename")]
        public async Task<IActionResult> RenameFile([FromBody] FileRenameDto fileRenameDto)
        {
            var fileRecord = await _fileRepository.GetFileRecord(fileRenameDto.UserId, fileRenameDto.OldFilename);
            if (fileRecord == null)
            {
                return NotFound();
            }

            fileRenameDto.NodeId = fileRecord.NodeId;
            var result = await _fileService.RenameFile(fileRenameDto);

            if (result.Success)
            {
                await _fileRepository.DeleteFileRecord(fileRenameDto.UserId, fileRenameDto.OldFilename);
                await _fileRepository.AddFileRecord(new FileRecord
                {
                    UserId = fileRenameDto.UserId,
                    Filename = fileRenameDto.NewFilename,
                    NodeId = fileRenameDto.NodeId,
                    Size = fileRecord.Size
                });
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
