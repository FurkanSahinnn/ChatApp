using ChatApp.Front.Models;

namespace ChatApp.Front.Interfaces
{
    public interface IAdminService
    {
        Task<List<UserModel>> GetAllUsersAsync();
        Task<UserModel?> GetUserByIdAsync(int id);
        Task<bool> CreateUserAsync(UserModel user);
        Task<bool> UpdateUserAsync(UserModel user);
        Task<bool> DeleteUserAsync(int id);
    }
}
