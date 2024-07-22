using DecentraCloud.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DecentraCloud.API.Interfaces.RepositoryInterfaces
{
    public interface IFileRepository
    {
        Task AddFileRecord(FileRecord fileRecord);
        Task<FileRecord> GetFileRecord(string userId, string filename);
        Task<IEnumerable<FileRecord>> SearchFileRecords(string userId, string query);
        Task<bool> DeleteFileRecord(string userId, string filename);
        Task<IEnumerable<FileRecord>> GetFilesByUserId(string userId);
        Task<FileRecord> GetFileByFilename(string filename);
    }
}
