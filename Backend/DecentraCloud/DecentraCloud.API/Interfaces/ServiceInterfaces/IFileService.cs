﻿using DecentraCloud.API.DTOs;
using DecentraCloud.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DecentraCloud.API.Interfaces.ServiceInterfaces
{
    public interface IFileService
    {
        Task<FileOperationResult> UploadFile(FileUploadDto fileUploadDto);
        Task<IEnumerable<FileRecord>> GetAllFiles(string userId);
        Task<byte[]> ViewFile(string userId, string fileId);
        Task<FileContentDto> DownloadFile(string userId, string fileId);
        Task<FileRecord> GetFile(string fileId);
        Task<IEnumerable<FileRecord>> SearchFiles(string userId, string query);
    }
}
