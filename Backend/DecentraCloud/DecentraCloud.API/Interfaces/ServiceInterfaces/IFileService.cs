using DecentraCloud.API.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DecentraCloud.API.Interfaces.ServiceInterfaces
{
    public interface IFileService
    {
        Task<FileOperationResult> UploadFile(FileUploadDto fileUploadDto);
        Task<FileOperationResult> DeleteFile(FileOperationDto fileOperationDto);
        Task<FileContentDto> ViewFile(FileOperationDto fileOperationDto);
        Task<FileContentDto> DownloadFile(FileOperationDto fileOperationDto);
        Task<IEnumerable<FileSearchResultDto>> SearchFiles(FileSearchDto fileSearchDto);
        Task<FileOperationResult> RenameFile(FileRenameDto fileRenameDto);
    }
}
