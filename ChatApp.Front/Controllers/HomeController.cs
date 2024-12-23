using ChatApp.Front.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Front.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly string _claimUrl = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/";
        public IActionResult Index()
        {
            // User = ClaimPrincipal
            // Kullanıcının oturum bilgilerinden (ClaimsPrincipal) username ve email’i alıyoruz
            var username = User.Claims.FirstOrDefault(c => c.Type == _claimUrl + "name")?.Value;
            var email = User.Claims.FirstOrDefault(c => c.Type == _claimUrl + "emailaddress")?.Value;

            // ViewData veya ViewBag kullanarak bilgileri Index.cshtml’e aktarabiliriz
            ViewData["Username"] = username ?? "Username not found";
            ViewData["Email"] = email ?? "Email not found";

            return View();
        }
        // member123456@gmail.com
        // Member123456
        public IActionResult HomePage()
        {
            // User = ClaimPrincipal
            // Kullanıcının oturum bilgilerinden (ClaimsPrincipal) username ve email’i alıyoruz
            var username = User.Claims.FirstOrDefault(c => c.Type == _claimUrl + "name")?.Value;
            var email = User.Claims.FirstOrDefault(c => c.Type == _claimUrl + "emailaddress")?.Value;

            // ViewData veya ViewBag kullanarak bilgileri Index.cshtml’e aktarabiliriz
            ViewData["Username"] = username ?? "Username not found";
            ViewData["Email"] = email ?? "Email not found";

            return View();
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
