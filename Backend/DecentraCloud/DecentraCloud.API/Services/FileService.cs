using DecentraCloud.API.DTOs;
using DecentraCloud.API.Helpers;
using DecentraCloud.API.Interfaces.RepositoryInterfaces;
using DecentraCloud.API.Interfaces.ServiceInterfaces;
using DecentraCloud.API.Models;


namespace DecentraCloud.API.Services
{
    public class FileService : IFileService
    {
        private readonly INodeService _nodeService;
        private readonly IFileRepository _fileRepository;
        private readonly IUserRepository _userRepository;
        private readonly EncryptionHelper _encryptionHelper;

        public FileService(INodeService nodeService, IFileRepository fileRepository, IUserRepository userRepository, EncryptionHelper encryptionHelper)
        {
            _nodeService = nodeService;
            _fileRepository = fileRepository;
            _userRepository = userRepository;
            _encryptionHelper = encryptionHelper;
        }

        public async Task<FileOperationResult> UploadFile(FileUploadDto fileUploadDto)
        {
            // Encrypt the file data before upload
            fileUploadDto.Data = _encryptionHelper.Encrypt(fileUploadDto.Data);

            // Randomly select a node
            var node = await _nodeService.GetRandomNode();
            fileUploadDto.NodeId = node.Id;

            var result = await _nodeService.UploadFileToNode(fileUploadDto);

            // Update the user's used storage space in the database
            if (result)
            {
                await _fileRepository.AddFileRecord(new FileRecord
                {
                    UserId = fileUploadDto.UserId,
                    Filename = fileUploadDto.Filename,
                    NodeId = fileUploadDto.NodeId,
                    Size = fileUploadDto.Data.Length
                });
                await _userRepository.UpdateUserStorageUsage(fileUploadDto.UserId, fileUploadDto.Data.Length);
            }

            return new FileOperationResult { Success = result, Message = "File uploaded successfully" };
        }

        public async Task<FileContentDto> ViewFile(FileOperationDto fileOperationDto)
        {
            var content = await _nodeService.GetFileContentFromNode(fileOperationDto);

            if (content != null)
            {
                // Decrypt the file content
                var decryptedContent = _encryptionHelper.Decrypt(Convert.FromBase64String(content));
                return new FileContentDto { Filename = fileOperationDto.Filename, Content = decryptedContent };
            }

            return null;
        }

        public async Task<FileContentDto> DownloadFile(FileOperationDto fileOperationDto)
        {
            var content = await _nodeService.GetFileContentFromNode(fileOperationDto);

            if (content != null)
            {
                // Decrypt the file content
                var decryptedContent = _encryptionHelper.Decrypt(Convert.FromBase64String(content));
                return new FileContentDto { Filename = fileOperationDto.Filename, Content = decryptedContent };
            }

            return null;
        }

        public async Task<FileOperationResult> DeleteFile(FileOperationDto fileOperationDto)
        {
            var result = await _nodeService.DeleteFileFromNode(fileOperationDto);

            // Update the user's used storage space in the database
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
                        Size = fileRecord.Size // Use the size from the existing file record
                    });
                }
                return new FileOperationResult { Success = true, Message = "File renamed successfully" };
            }

            return new FileOperationResult { Success = false, Message = "Failed to rename file" };
        }
    }
}
