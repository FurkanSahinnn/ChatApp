using ChatApp.Front.Models;
using ChatApp.Front.TwoFactorService;
using ChatApp.Front.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ChatApp.Front.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly string _claimUrl = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/";
        public IActionResult HomePage()
        {

            if (Request.Cookies.TryGetValue("JWTToken", out var token))
            {
                var handler = new JwtSecurityTokenHandler();
                if (handler.CanReadToken(token))
                {
                    var jwtToken = handler.ReadJwtToken(token);
                    var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                   
                    ViewData["UserId"] = int.Parse(userId);
                    return View();
                }
            }
            return Unauthorized();
        }

        [Authorize]
        public IActionResult RedirectToProfile()
        {
            // Cookie'deki token'ı al
            if (Request.Cookies.TryGetValue("JWTToken", out var token))
            {
                var handler = new JwtSecurityTokenHandler();

                if (handler.CanReadToken(token))
                {
                    var jwtToken = handler.ReadJwtToken(token);
                    var claims = jwtToken.Claims;

                    var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                    var username = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                    var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                    var role = RoleParser.ParseStringToInt(claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value); // 1 -> "admin", 2 -> "member"
                    
                    var userModel = new UserModel
                    {
                        Id = int.Parse(userId),
                        Name = username,
                        Email = email,
                        RoleId = role,
                    };

                    return RedirectToAction("Profile", userModel);
                }
            } 
            
            return Unauthorized();
        }

        [Authorize]
        public IActionResult Profile(UserModel model)
        {
            return View(model);
        }

        // Logout işlemi
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account"); 
        }
    }
}
