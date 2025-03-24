using KnihovnaAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnihovnaAPI.Services
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByNameAsync(string name);
        Task<User> AuthenticateAsync(string username, string password);
        Task<int> CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
    }
} 