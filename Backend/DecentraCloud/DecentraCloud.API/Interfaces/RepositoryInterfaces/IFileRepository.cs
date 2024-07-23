using DecentraCloud.API.DTOs;
using DecentraCloud.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DecentraCloud.API.Interfaces.RepositoryInterfaces
{
    public interface IFileRepository
    {
        Task AddFileRecord(FileRecord fileRecord);
        Task<FileRecord> GetFileRecord(string userId, string filename);
        Task<FileRecord> GetFileRecordById(string fileId);
        Task<IEnumerable<FileRecord>> GetFilesByUserId(string userId);
        Task<bool> DeleteFileRecord(string userId, string filename);

        // New methods for interacting with storage nodes
        Task<bool> UploadFileToNode(FileUploadDto fileUploadDto, Node node);
        Task<byte[]> DownloadFileFromNode(string userId, string fileId, Node node);
        Task<byte[]> ViewFileOnNode(string userId, string fileId, Node node);
        Task<IEnumerable<FileRecord>> SearchFileRecords(string userId, string query);
    }
}
