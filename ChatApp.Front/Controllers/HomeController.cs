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
    }
}
