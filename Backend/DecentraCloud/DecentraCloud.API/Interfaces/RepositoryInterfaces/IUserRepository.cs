using DecentraCloud.API.Models;
using System.Threading.Tasks;

namespace DecentraCloud.API.Interfaces.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task<User> RegisterUser(User user);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserById(string userId);
    }
}
