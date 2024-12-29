using ChatApp.Front.Models;
using ChatApp.Front.TwoFactorService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ChatApp.Front.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly string _claimUrl = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/";
        // member123456@gmail.com
        // Member123456
        public IActionResult HomePage()
        {
            // Cookie'deki token'ı al
            if (Request.Cookies.TryGetValue("JWTToken", out var token))
            {
                var handler = new JwtSecurityTokenHandler();

                if (handler.CanReadToken(token))
                {
                    var jwtToken = handler.ReadJwtToken(token);
                    var claims = jwtToken.Claims;

                    // Örnek: Kullanıcı adı ve email al
                    var username = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                    var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                    ViewData["Username"] = username ?? "Username not found";
                    ViewData["Email"] = email ?? "Email not found";

                    return View();
                }
            }

            return Unauthorized();
            /*
            // User = ClaimPrincipal
            // Kullanıcının oturum bilgilerinden (ClaimsPrincipal) username ve email’i alıyoruz
            var username = User.Claims.FirstOrDefault(c => c.Type == _claimUrl + "name")?.Value;
            var email = User.Claims.FirstOrDefault(c => c.Type == _claimUrl + "emailaddress")?.Value;

            // ViewData veya ViewBag kullanarak bilgileri Index.cshtml’e aktarabiliriz
            ViewData["Username"] = username ?? "Username not found";
            ViewData["Email"] = email ?? "Email not found";

            return View();
            */
        }

        [Authorize]
        public IActionResult RedirectToProfile()
        {
            // Kullanıcı bilgilerini al (claims üzerinden)
            var username = User.Claims.FirstOrDefault(c => c.Type == _claimUrl + "name")?.Value;
            var email = User.Claims.FirstOrDefault(c => c.Type == _claimUrl + "emailaddress")?.Value;

            // Kullanıcı bilgilerini UserModel ile yönlendirme sırasında gönder
            var userModel = new UserModel
            {
                UserName = username,
                Email = email,
                ProfileImageUrl = "/images/default-profile.png" // Varsayılan profil resmi
            };

            return RedirectToAction("Profile", userModel);
        }

        [Authorize]
        public IActionResult Profile(UserModel model)
        {
            // Profile sayfasında bilgileri model üzerinden kullanabilirsiniz
            return View(model);
        }

        // Logout işlemi
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account"); // Kullanıcı login sayfasına yönlendirilir
        }
    }
}
