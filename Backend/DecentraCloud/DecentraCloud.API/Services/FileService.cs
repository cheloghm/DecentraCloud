using DecentraCloud.API.DTOs;
using DecentraCloud.API.Interfaces.ServiceInterfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DecentraCloud.API.Services
{
    public class FileService : IFileService
    {
        private readonly INodeService _nodeService;

        public FileService(INodeService nodeService)
        {
            _nodeService = nodeService;
        }

        public async Task<FileOperationResult> UploadFile(FileUploadDto fileUploadDto)
        {
            var result = await _nodeService.UploadFileToNode(fileUploadDto);
            return new FileOperationResult { Success = result, Message = "File uploaded successfully" };
        }

        public async Task<FileOperationResult> DeleteFile(FileOperationDto fileOperationDto)
        {
            var result = await _nodeService.DeleteFileFromNode(fileOperationDto);
            return new FileOperationResult { Success = result, Message = "File deleted successfully" };
        }

        public async Task<FileContentDto> ViewFile(FileOperationDto fileOperationDto)
        {
            var content = await _nodeService.GetFileContentFromNode(fileOperationDto);
            return new FileContentDto { Filename = fileOperationDto.Filename, Content = content };
        }

        public async Task<FileContentDto> DownloadFile(FileOperationDto fileOperationDto)
        {
            var content = await _nodeService.GetFileContentFromNode(fileOperationDto);
            return new FileContentDto { Filename = fileOperationDto.Filename, Content = content };
        }

        public async Task<IEnumerable<FileSearchResultDto>> SearchFiles(FileSearchDto fileSearchDto)
        {
            var results = await _nodeService.SearchFilesInNode(fileSearchDto);
            return results;
        }
    }
}
