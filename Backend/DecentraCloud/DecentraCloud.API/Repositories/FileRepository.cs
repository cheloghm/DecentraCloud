using DecentraCloud.API.Data;
using DecentraCloud.API.Interfaces.RepositoryInterfaces;
using DecentraCloud.API.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DecentraCloud.API.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly DecentraCloudContext _context;

        public FileRepository(DecentraCloudContext context)
        {
            _context = context;
        }

        public async Task AddFileRecord(FileRecord fileRecord)
        {
            await _context.Files.InsertOneAsync(fileRecord);
        }

        public async Task<FileRecord> GetFileRecord(string userId, string filename)
        {
            var filter = Builders<FileRecord>.Filter.And(
                Builders<FileRecord>.Filter.Eq(f => f.UserId, userId),
                Builders<FileRecord>.Filter.Eq(f => f.Filename, filename)
            );
            return await _context.Files.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<FileRecord>> SearchFileRecords(string userId, string query)
        {
            var filter = Builders<FileRecord>.Filter.And(
                Builders<FileRecord>.Filter.Eq(f => f.UserId, userId),
                Builders<FileRecord>.Filter.Regex(f => f.Filename, new MongoDB.Bson.BsonRegularExpression(query, "i"))
            );
            return await _context.Files.Find(filter).ToListAsync();
        }

        public async Task<bool> DeleteFileRecord(string userId, string filename)
        {
            var filter = Builders<FileRecord>.Filter.And(
                Builders<FileRecord>.Filter.Eq(f => f.UserId, userId),
                Builders<FileRecord>.Filter.Eq(f => f.Filename, filename)
            );
            var result = await _context.Files.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }

        public async Task<IEnumerable<FileRecord>> GetFilesByUserId(string userId)
        {
            return await _context.Files.Find(f => f.UserId == userId).ToListAsync();
        }

        public async Task<FileRecord> GetFileByFilename(string filename)
        {
            return await _context.Files.Find(f => f.Filename == filename).FirstOrDefaultAsync();
        }

        public async Task<FileRecord> GetFileRecordByOriginalFilename(string userId, string originalFilename)
        {
            var filter = Builders<FileRecord>.Filter.And(
                Builders<FileRecord>.Filter.Eq(f => f.UserId, userId),
                Builders<FileRecord>.Filter.Eq(f => f.OriginalFilename, originalFilename)
            );
            return await _context.Files.Find(filter).FirstOrDefaultAsync();
        }

    }
}
