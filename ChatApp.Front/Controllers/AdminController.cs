using ChatApp.Front.Interfaces;
using ChatApp.Front.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Front.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<IActionResult> HomePage() {
            var users = await _adminService.GetAllUsersAsync();
            return View(users); 
        }
    }
}
