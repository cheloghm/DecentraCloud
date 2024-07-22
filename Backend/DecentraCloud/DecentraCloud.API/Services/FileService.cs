using DecentraCloud.API.DTOs;
using DecentraCloud.API.Helpers;
using DecentraCloud.API.Interfaces.RepositoryInterfaces;
using DecentraCloud.API.Interfaces.ServiceInterfaces;
using DecentraCloud.API.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
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

        private HttpClient CreateHttpClient()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
            };

            return new HttpClient(handler);
        }

        public async Task<FileOperationResult> UploadFile(FileUploadDto fileUploadDto)
        {
            // Encrypt the file data before upload
            fileUploadDto.Data = _encryptionHelper.Encrypt(fileUploadDto.Data);

            // Randomly select a node that is online and has a valid endpoint
            var node = await _nodeService.GetRandomOnlineNode();
            if (node == null)
            {
                return new FileOperationResult { Success = false, Message = "No available nodes for upload." };
            }
            fileUploadDto.NodeId = node.Id;

            // Add file record to the database first to generate the file ID
            var fileRecord = new FileRecord
            {
                UserId = fileUploadDto.UserId,
                Filename = fileUploadDto.Filename, // Original filename
                NodeId = fileUploadDto.NodeId,
                Size = fileUploadDto.Data.Length,
                DateAdded = DateTime.UtcNow
            };
            await _fileRepository.AddFileRecord(fileRecord);

            // Use the generated file ID as the filename for the storage node
            var fileId = fileRecord.Id;

            var result = await _nodeService.UploadFileToNode(new FileUploadDto
            {
                UserId = fileUploadDto.UserId,
                Filename = fileId, // Use file ID as filename on storage node
                Data = fileUploadDto.Data,
                NodeId = fileUploadDto.NodeId
            });

            if (result)
            {
                await _userRepository.UpdateUserStorageUsage(fileUploadDto.UserId, fileUploadDto.Data.Length);
            }

            return new FileOperationResult { Success = result, Message = result ? "File uploaded successfully" : "File upload failed" };
        }

        public async Task<IEnumerable<FileRecord>> GetAllFiles(string userId)
        {
            return await _fileRepository.GetFilesByUserId(userId);
        }

        public async Task<byte[]> ViewFile(string userId, string fileId)
        {
            var fileRecord = await _fileRepository.GetFileRecordById(fileId);
            if (fileRecord == null || fileRecord.UserId != userId)
            {
                return null;
            }

            var node = await _nodeService.GetNodeById(fileRecord.NodeId);
            if (node == null || !node.IsOnline || string.IsNullOrEmpty(node.Endpoint))
            {
                return null;
            }

            var httpClient = CreateHttpClient();
            var url = $"{node.Endpoint}/storage/view/{userId}/{fileId}";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", node.Token);
            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var encryptedContent = await response.Content.ReadAsByteArrayAsync();
            return _encryptionHelper.Decrypt(encryptedContent);
        }


        public async Task<FileContentDto> DownloadFile(string userId, string fileId)
        {
            var fileRecord = await _fileRepository.GetFileRecordById(fileId);
            if (fileRecord == null || fileRecord.UserId != userId)
            {
                return null;
            }

            var node = await _nodeService.GetNodeById(fileRecord.NodeId);
            if (node == null || !node.IsOnline || string.IsNullOrEmpty(node.Endpoint))
            {
                return null;
            }

            var httpClient = CreateHttpClient();
            var url = $"{node.Endpoint}/storage/download/{userId}/{fileId}";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", node.Token);
            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var encryptedContent = await response.Content.ReadAsByteArrayAsync();
            var decryptedContent = _encryptionHelper.Decrypt(encryptedContent);
            return new FileContentDto
            {
                Filename = fileRecord.Filename,
                Content = decryptedContent
            };
        }

        public async Task<FileRecord> GetFile(string fileId)
        {
            return await _fileRepository.GetFileRecordById(fileId);
        }
    }
}
