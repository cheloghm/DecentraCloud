using DecentraCloud.API.DTOs;
using DecentraCloud.API.Helpers;
using DecentraCloud.API.Interfaces.RepositoryInterfaces;
using DecentraCloud.API.Interfaces.ServiceInterfaces;
using DecentraCloud.API.Models;
using System;
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

            // Generate UUID for the file
            var uuid = Guid.NewGuid().ToString();
            fileUploadDto.Filename = uuid;

            // Randomly select a node
            var node = await _nodeService.GetRandomNode();
            fileUploadDto.NodeId = node.Id;

            var result = await _nodeService.UploadFileToNode(fileUploadDto);

            if (result)
            {
                await _fileRepository.AddFileRecord(new FileRecord
                {
                    UserId = fileUploadDto.UserId,
                    Filename = uuid,
                    OriginalFilename = fileUploadDto.OriginalFilename,
                    NodeId = fileUploadDto.NodeId,
                    Size = fileUploadDto.Data.Length,
                    Uuid = uuid
                });

                await _userRepository.UpdateUserStorageUsage(fileUploadDto.UserId, fileUploadDto.Data.Length);
            }

            return new FileOperationResult { Success = result, Message = result ? "File uploaded successfully" : "File upload failed" };
        }

    }
}
