using DecentraCloud.API.DTOs;
using DecentraCloud.API.Helpers;
using DecentraCloud.API.Interfaces.RepositoryInterfaces;
using DecentraCloud.API.Interfaces.ServiceInterfaces;
using DecentraCloud.API.Models;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DecentraCloud.API.Services
{
    public class FileService : IFileService
    {
        private readonly INodeService _nodeService;
        private readonly IFileRepository _fileRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFilePermissionRepository _filePermissionRepository;
        private readonly EncryptionHelper _encryptionHelper;

        public FileService(INodeService nodeService, IFileRepository fileRepository, IUserRepository userRepository, IFilePermissionRepository filePermissionRepository, EncryptionHelper encryptionHelper)
        {
            _nodeService = nodeService;
            _fileRepository = fileRepository;
            _userRepository = userRepository;
            _filePermissionRepository = filePermissionRepository;
            _encryptionHelper = encryptionHelper;
        }

        public async Task<FileOperationResult> UploadFile(FileUploadDto fileUploadDto)
        {
            // Encrypt the file data before upload
            fileUploadDto.Data = _encryptionHelper.Encrypt(fileUploadDto.Data);

            // Encrypt the filename but keep the original filename
            var encryptedFilename = Convert.ToBase64String(_encryptionHelper.Encrypt(Encoding.UTF8.GetBytes(fileUploadDto.Filename)));
            fileUploadDto.Filename = encryptedFilename;

            // Randomly select a node
            var node = await _nodeService.GetRandomNode();
            fileUploadDto.NodeId = node.Id;

            var result = await _nodeService.UploadFileToNode(fileUploadDto);

            if (result)
            {
                await _fileRepository.AddFileRecord(new FileRecord
                {
                    UserId = fileUploadDto.UserId,
                    Filename = fileUploadDto.Filename,
                    OriginalFilename = fileUploadDto.OriginalFilename, // Store the original filename as is
                    NodeId = fileUploadDto.NodeId,
                    Size = fileUploadDto.Data.Length
                });
                await _userRepository.UpdateUserStorageUsage(fileUploadDto.UserId, fileUploadDto.Data.Length);
            }

            return new FileOperationResult { Success = result, Message = result ? "File uploaded successfully" : "File upload failed" };
        }

        public async Task<FileContentDto> ViewFile(FileOperationDto fileOperationDto)
        {
            var encryptedFilename = Convert.ToBase64String(_encryptionHelper.Encrypt(Encoding.UTF8.GetBytes(fileOperationDto.Filename)));
            fileOperationDto.Filename = encryptedFilename;

            var content = await _nodeService.GetFileContentFromNode(fileOperationDto);

            if (content != null)
            {
                var decryptedContent = _encryptionHelper.Decrypt(Convert.FromBase64String(content));
                return new FileContentDto { Filename = fileOperationDto.Filename, Content = decryptedContent };
            }

            return null;
        }

        public async Task<FileContentDto> DownloadFile(FileOperationDto fileOperationDto)
        {
            var encryptedFilename = Convert.ToBase64String(_encryptionHelper.Encrypt(Encoding.UTF8.GetBytes(fileOperationDto.Filename)));
            fileOperationDto.Filename = encryptedFilename;

            var content = await _nodeService.GetFileContentFromNode(fileOperationDto);

            if (content != null)
            {
                var decryptedContent = _encryptionHelper.Decrypt(Convert.FromBase64String(content));
                return new FileContentDto { Filename = fileOperationDto.Filename, Content = decryptedContent };
            }

            return null;
        }

        public async Task<FileOperationResult> DeleteFile(FileOperationDto fileOperationDto)
        {
            var encryptedFilename = Convert.ToBase64String(_encryptionHelper.Encrypt(Encoding.UTF8.GetBytes(fileOperationDto.Filename)));
            fileOperationDto.Filename = encryptedFilename;

            var result = await _nodeService.DeleteFileFromNode(fileOperationDto);

            if (result)
            {
                var fileRecord = await _fileRepository.GetFileRecord(fileOperationDto.UserId, fileOperationDto.Filename);
                if (fileRecord != null)
                {
                    await _fileRepository.DeleteFileRecord(fileOperationDto.UserId, fileOperationDto.Filename);
                    await _userRepository.UpdateUserStorageUsage(fileOperationDto.UserId, -fileRecord.Size);
                }
            }

            return new FileOperationResult { Success = result, Message = "File deleted successfully" };
        }

        public async Task<IEnumerable<FileSearchResultDto>> SearchFiles(FileSearchDto fileSearchDto)
        {
            var results = await _nodeService.SearchFilesInNode(fileSearchDto);
            return results;
        }

        public async Task<FileOperationResult> RenameFile(FileRenameDto fileRenameDto)
        {
            var result = await _nodeService.RenameFileInNode(fileRenameDto);

            if (result)
            {
                var fileRecord = await _fileRepository.GetFileRecord(fileRenameDto.UserId, fileRenameDto.OldFilename);
                if (fileRecord != null)
                {
                    await _fileRepository.DeleteFileRecord(fileRenameDto.UserId, fileRenameDto.OldFilename);
                    await _fileRepository.AddFileRecord(new FileRecord
                    {
                        UserId = fileRenameDto.UserId,
                        Filename = fileRenameDto.NewFilename,
                        NodeId = fileRenameDto.NodeId,
                        Size = fileRecord.Size
                    });
                }
                return new FileOperationResult { Success = true, Message = "File renamed successfully" };
            }

            return new FileOperationResult { Success = false, Message = "Failed to rename file" };
        }

        public async Task<FileOperationResult> ShareFile(FileShareDto fileShareDto)
        {
            var fileRecord = await _fileRepository.GetFileRecord(fileShareDto.OwnerId, fileShareDto.Filename);
            if (fileRecord == null)
            {
                return new FileOperationResult { Success = false, Message = "File not found" };
            }

            var sharedUser = await _userRepository.GetUserByEmail(fileShareDto.Email);
            if (sharedUser == null)
            {
                return new FileOperationResult { Success = false, Message = "User not found" };
            }

            var filePermission = new FilePermission
            {
                FileId = fileRecord.Id,
                OwnerId = fileShareDto.OwnerId,
                UserId = sharedUser.Id,
                PermissionType = fileShareDto.PermissionType
            };

            await _filePermissionRepository.AddFilePermission(filePermission);
            return new FileOperationResult { Success = true, Message = "File shared successfully" };
        }

        public async Task<FileOperationResult> RevokeFilePermission(FileRevokePermissionDto fileRevokePermissionDto)
        {
            var filePermission = await _filePermissionRepository.GetFilePermission(fileRevokePermissionDto.FileId, fileRevokePermissionDto.UserId);
            if (filePermission == null)
            {
                return new FileOperationResult { Success = false, Message = "Permission not found" };
            }

            await _filePermissionRepository.DeleteFilePermission(filePermission.Id);
            return new FileOperationResult { Success = true, Message = "Permission revoked successfully" };
        }

        public async Task<IEnumerable<FilePermissionDto>> GetFilePermissions(string fileId)
        {
            var permissions = await _filePermissionRepository.GetFilePermissions(fileId);
            return permissions.Select(p => new FilePermissionDto
            {
                FileId = p.FileId,
                UserId = p.UserId,
                PermissionType = p.PermissionType
            });
        }

        public async Task<bool> HasPermission(string userId, string fileId, string permissionType)
        {
            var permissions = await _filePermissionRepository.GetFilePermissions(fileId);
            return permissions.Any(p => p.UserId == userId && p.PermissionType == permissionType);
        }

        public async Task<FileOperationResult> MoveFile(FileMoveDto fileMoveDto)
        {
            var deleteResult = await DeleteFile(new FileOperationDto
            {
                UserId = fileMoveDto.UserId,
                Filename = fileMoveDto.OldFilename,
                NodeId = fileMoveDto.OldNodeId
            });

            if (!deleteResult.Success)
            {
                return new FileOperationResult { Success = false, Message = "Failed to move file" };
            }

            var fileUploadDto = new FileUploadDto
            {
                UserId = fileMoveDto.UserId,
                Filename = fileMoveDto.NewFilename,
                Data = fileMoveDto.Data,
                NodeId = fileMoveDto.NewNodeId
            };

            var uploadResult = await UploadFile(fileUploadDto);

            return uploadResult;
        }

        public async Task<FileOperationResult> CopyFile(FileCopyDto fileCopyDto)
        {
            var fileContentDto = await DownloadFile(new FileOperationDto
            {
                UserId = fileCopyDto.UserId,
                Filename = fileCopyDto.Filename,
                NodeId = fileCopyDto.NodeId
            });

            if (fileContentDto == null)
            {
                return new FileOperationResult { Success = false, Message = "Failed to copy file" };
            }

            var fileUploadDto = new FileUploadDto
            {
                UserId = fileCopyDto.UserId,
                Filename = fileCopyDto.NewFilename,
                Data = fileContentDto.Content,
                NodeId = fileCopyDto.NewNodeId
            };

            var uploadResult = await UploadFile(fileUploadDto);

            return uploadResult;
        }

        public async Task<FileOperationResult> SaveFileVersion(FileVersionDto fileVersionDto)
        {
            var fileContentDto = await DownloadFile(new FileOperationDto
            {
                UserId = fileVersionDto.UserId,
                Filename = fileVersionDto.Filename,
                NodeId = fileVersionDto.NodeId
            });

            if (fileContentDto == null)
            {
                return new FileOperationResult { Success = false, Message = "Failed to save file version" };
            }

            var versionedFilename = $"{fileVersionDto.Filename}_v{fileVersionDto.VersionNumber}";
            var fileUploadDto = new FileUploadDto
            {
                UserId = fileVersionDto.UserId,
                Filename = versionedFilename,
                Data = fileContentDto.Content,
                NodeId = fileVersionDto.NodeId
            };

            var uploadResult = await UploadFile(fileUploadDto);

            return uploadResult;
        }
    }
}
