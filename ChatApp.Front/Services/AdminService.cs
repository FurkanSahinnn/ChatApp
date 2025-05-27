using ChatApp.Front.Interfaces;
using ChatApp.Front.Models;
using System.Text.Json;

namespace ChatApp.Front.Services
{
    public class AdminService : IAdminService
    {
        private readonly HttpClient _httpClient;

        //private readonly string BASE_URL = "http://localhost:5221/api/admin";

        public AdminService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("WebApiClient");
        }

        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            var response = await _httpClient.GetAsync("http://localhost:5221/api/admin/users");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<UserModel>>() ?? new List<UserModel>();
            }
            throw new Exception("Failed to fetch users from the API.");
        }

        public async Task<UserModel?> GetUserByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/admin/users/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserModel>();
            }
            return null;
        }

        public async Task<bool> CreateUserAsync(UserModel user)
        {
            var response = await _httpClient.PostAsJsonAsync("api/admin/users", user);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateUserAsync(UserModel user)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/admin/users/{user.Id}", user);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/admin/users/{id}");
            return response.IsSuccessStatusCode;
        }
    }

}
