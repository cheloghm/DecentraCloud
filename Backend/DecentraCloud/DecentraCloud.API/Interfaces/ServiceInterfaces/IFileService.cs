using System.Collections.Generic;
using System.Threading.Tasks;
using DecentraCloud.API.DTOs;
using DecentraCloud.API.Models;

namespace DecentraCloud.API.Interfaces.ServiceInterfaces
{
    public interface IFileService
    {
        Task<FileOperationResult> UploadFile(FileUploadDto fileUploadDto);
        Task<FileContentDto> ViewFile(FileOperationDto fileOperationDto);
        Task<FileContentDto> DownloadFile(FileOperationDto fileOperationDto);
        Task<FileOperationResult> DeleteFile(FileOperationDto fileOperationDto);
        Task<IEnumerable<FileSearchResultDto>> SearchFiles(FileSearchDto fileSearchDto);
        Task<FileOperationResult> RenameFile(FileRenameDto fileRenameDto);
        Task<FileOperationResult> MoveFile(FileMoveDto fileMoveDto);
        Task<FileOperationResult> CopyFile(FileCopyDto fileCopyDto);
        Task<FileOperationResult> SaveFileVersion(FileVersionDto fileVersionDto);
        Task<FileOperationResult> ShareFile(FileShareDto fileShareDto);
        Task<FileOperationResult> RevokeFilePermission(FileRevokePermissionDto fileRevokePermissionDto);
        Task<IEnumerable<FilePermissionDto>> GetFilePermissions(string fileId);
        Task<bool> HasPermission(string userId, string fileId, string permissionType);
    }
}
