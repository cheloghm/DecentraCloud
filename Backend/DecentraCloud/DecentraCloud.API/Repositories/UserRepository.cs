using DecentraCloud.API.Data;
using DecentraCloud.API.Interfaces.RepositoryInterfaces;
using DecentraCloud.API.Models;
using MongoDB.Driver;

namespace DecentraCloud.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DecentraCloudContext _context;

        public UserRepository(DecentraCloudContext context)
        {
            _context = context;
        }

        public async Task<User> RegisterUser(User user)
        {
            await _context.Users.InsertOneAsync(user);
            return user;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.Find(u => u.Email == email).FirstOrDefaultAsync();
        }
    }
}
