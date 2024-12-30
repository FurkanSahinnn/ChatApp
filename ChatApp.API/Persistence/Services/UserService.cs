using ChatApp.API.Core.Application.Interfaces;
using ChatApp.API.Core.Domain;

namespace ChatApp.API.Persistence.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<UserApp> _userRepository;

        public UserService(IGenericRepository<UserApp> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserApp>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<UserApp?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<UserApp?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.WhereAsync(u => u.Email == email);
        }

        public async Task CreateUserAsync(UserApp user)
        {
            await _userRepository.CreateAsync(user);
        }

        public async Task UpdateUserAsync(UserApp user)
        {
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user != null)
            {
                await _userRepository.DeleteAsync(user);
            }
        }
    }

}
