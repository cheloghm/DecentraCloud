using DecentraCloud.API.Data;
using DecentraCloud.API.Interfaces.RepositoryInterfaces;
using DecentraCloud.API.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DecentraCloud.API.Repositories
{
    public class FilePermissionRepository : IFilePermissionRepository
    {
        private readonly DecentraCloudContext _context;

        public FilePermissionRepository(DecentraCloudContext context)
        {
            _context = context;
        }

        public async Task AddFilePermission(FilePermission filePermission)
        {
            await _context.FilePermissions.InsertOneAsync(filePermission);
        }

        public async Task DeleteFilePermission(string permissionId)
        {
            await _context.FilePermissions.DeleteOneAsync(permission => permission.Id == permissionId);
        }

        public async Task<FilePermission> GetFilePermission(string fileId, string userId)
        {
            var filter = Builders<FilePermission>.Filter.And(
                Builders<FilePermission>.Filter.Eq(permission => permission.FileId, fileId),
                Builders<FilePermission>.Filter.Eq(permission => permission.UserId, userId)
            );
            return await _context.FilePermissions.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<FilePermission>> GetFilePermissions(string fileId)
        {
            return await _context.FilePermissions.Find(permission => permission.FileId == fileId).ToListAsync();
        }
    }
}
