using ChatApp.API.Core.Domain;

namespace ChatApp.API.Core.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<UserApp>> GetAllUsersAsync();
        Task<UserApp?> GetUserByIdAsync(int id);

        Task<UserApp?> GetUserByEmailAsync(string email);
        Task CreateUserAsync(UserApp user);
        Task UpdateUserAsync(UserApp user);
        Task DeleteUserAsync(int id);
    }
}
