using DecentraCloud.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DecentraCloud.API.Interfaces.RepositoryInterfaces
{
    public interface IFilePermissionRepository
    {
        Task AddFilePermission(FilePermission filePermission);
        Task DeleteFilePermission(string permissionId);
        Task<FilePermission> GetFilePermission(string fileId, string userId);
        Task<IEnumerable<FilePermission>> GetFilePermissions(string fileId);
    }
}
