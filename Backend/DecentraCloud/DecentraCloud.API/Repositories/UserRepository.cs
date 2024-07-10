using DecentraCloud.API.Data;
using DecentraCloud.API.Interfaces.RepositoryInterfaces;
using DecentraCloud.API.Models;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace DecentraCloud.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(DecentraCloudContext context)
        {
            _users = context.Users;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _users.Find(user => user.Email == email).FirstOrDefaultAsync();
        }

        public async Task<User> RegisterUser(User user)
        {
            await _users.InsertOneAsync(user);
            return user;
        }

        public async Task<User> GetUserById(string userId)
        {
            return await _users.Find(user => user.Id == userId).FirstOrDefaultAsync();
        }

        public async Task UpdateUser(User user)
        {
            await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
        }

        public async Task<bool> DeleteUser(string userId)
        {
            var result = await _users.DeleteOneAsync(user => user.Id == userId);
            return result.DeletedCount > 0;
        }

        public async Task UpdateUserStorageUsage(string userId, long storageUsed)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var update = Builders<User>.Update.Inc(u => u.UsedStorage, storageUsed);
            await _users.UpdateOneAsync(filter, update);
        }
    }
}
