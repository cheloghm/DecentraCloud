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
        private readonly IMongoCollection<FileRecord> _files;

        public FileRepository(DecentraCloudContext context)
        {
            _files = context.Files;
        }

        public async Task AddFileRecord(FileRecord fileRecord)
        {
            await _files.InsertOneAsync(fileRecord);
        }

        public async Task<FileRecord> GetFileRecord(string userId, string filename)
        {
            var filter = Builders<FileRecord>.Filter.And(
                Builders<FileRecord>.Filter.Eq(f => f.UserId, userId),
                Builders<FileRecord>.Filter.Eq(f => f.Filename, filename)
            );
            return await _files.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<FileRecord>> SearchFileRecords(string userId, string query)
        {
            var filter = Builders<FileRecord>.Filter.And(
                Builders<FileRecord>.Filter.Eq(f => f.UserId, userId),
                Builders<FileRecord>.Filter.Regex(f => f.Filename, new MongoDB.Bson.BsonRegularExpression(query, "i"))
            );
            return await _files.Find(filter).ToListAsync();
        }

        public async Task<bool> DeleteFileRecord(string userId, string filename)
        {
            var filter = Builders<FileRecord>.Filter.And(
                Builders<FileRecord>.Filter.Eq(f => f.UserId, userId),
                Builders<FileRecord>.Filter.Eq(f => f.Filename, filename)
            );
            var result = await _files.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
    }
}
