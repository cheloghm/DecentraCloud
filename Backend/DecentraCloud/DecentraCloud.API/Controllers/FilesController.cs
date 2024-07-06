using Microsoft.AspNetCore.Mvc;
using DecentraCloud.API.DTOs;
using DecentraCloud.API.Interfaces.ServiceInterfaces;
using System.Threading.Tasks;

namespace DecentraCloud.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly INodeService _nodeService;

        public FileController(IFileService fileService, INodeService nodeService)
        {
            _fileService = fileService;
            _nodeService = nodeService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromBody] FileUploadDto fileUploadDto)
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
            var result = await _fileService.DeleteFile(fileOperationDto);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("view")]
        public async Task<IActionResult> ViewFile([FromQuery] FileOperationDto fileOperationDto)
        {
            var result = await _fileService.ViewFile(fileOperationDto);

            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

        [HttpGet("download")]
        public async Task<IActionResult> DownloadFile([FromQuery] FileOperationDto fileOperationDto)
        {
            var result = await _fileService.DownloadFile(fileOperationDto);

            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchFiles([FromQuery] FileSearchDto fileSearchDto)
        {
            var results = await _fileService.SearchFiles(fileSearchDto);
            return Ok(results);
        }
    }
}
