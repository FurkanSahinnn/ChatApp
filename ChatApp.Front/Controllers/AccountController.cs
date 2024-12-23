using ChatApp.Front.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using ChatApp.Front.TwoFactorService;

namespace ChatApp.Front.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<TwoFactorOptions> _twoFactorOptions;

        public AccountController(IHttpClientFactory httpClientFactory, IOptions<TwoFactorOptions> twoFactorOptions)
        {
            _httpClientFactory = httpClientFactory;
            _twoFactorOptions = twoFactorOptions;
        }
        public IActionResult TwoFactor()
        {
            // Formu görüntüler
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TwoFactor(TwoFactorModel model)
        {
            if (TempData["registerModel"] == null)
            {
                // Eğer RegisterModel yoksa kullanıcıyı Register sayfasına geri yönlendir
                return RedirectToAction("Register");
            }
            var registerModel = TempData["registerModel"];

            // code verification yap.
            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(JsonSerializer.Serialize(registerModel), Encoding.UTF8, "application/json");

            // Web API'ye POST isteği gönder
            var response = await client.PostAsync("http://localhost:5221/api/auth/register", content);

            if (response.IsSuccessStatusCode)
            {
                // Başarılı kayıt sonrası giriş sayfasına yönlendir
                return RedirectToAction("Login", "Account");
            }
            else
            {
                // Hata mesajını ekrana yazdır
                ModelState.AddModelError("", "An error occurred while registering the user.");
                return View(model);
            }

        }

        public IActionResult Register()
        {
            return View(new RegisterModel());
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Model hataları varsa tekrar formu göster
            }
            //TempData["registerModel"] = model;
            // code verification yap.
            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            // Web API'ye POST isteği gönder
            var response = await client.PostAsync("http://localhost:5221/api/auth/register", content);

            if (response.IsSuccessStatusCode)
            {
                // Başarılı kayıt sonrası giriş sayfasına yönlendir
                return RedirectToAction("Login", "Account");
            }
            else
            {
                // Hata mesajını ekrana yazdır
                ModelState.AddModelError("", "An error occurred while registering the user.");
                return View(model);
            }
            //return RedirectToAction("TwoFactor");
        }

        public IActionResult Login()
        {
            // Formu görüntüler
            return View(new LoginModel());
        }  
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            // Kullanıcıdan gelen bilgilerin geçerli olduğundan emin ol
            if (ModelState.IsValid)
            {
                // Web API'ye HTTP POST isteği göndermek için bir HttpClient oluştur.
                var client = _httpClientFactory.CreateClient();

                // Web API, genellikle JSON formatında veri bekler,
                // bu yüzden kullanıcı bilgilerini Json olarak göndermeliyiz.
                var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

                // Kullanıcının giriş bilgilerini (e-posta ve şifre) API'ye gönderip
                // API üzerinden kullanıcının doğruluğunu kontrol edip JWT token döndürüp,
                // sonucu alıyoruz.
                var response = await client.PostAsync("http://localhost:5221/api/Auth/Login", content);

                // HTTP yanıt kodunun 200 (OK) serisinde olup olmadığını kontrol et.
                Console.WriteLine(response.IsSuccessStatusCode);
                if (response.IsSuccessStatusCode)
                {
                    // API'den dönen JSON içeriğini string olarak oku
                    // ve bu stringi TokenResponseModel adlı sınıfa dönüştürür.
                    var tokenModel = JsonSerializer.Deserialize<TokenResponseModel>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                    
                    if (tokenModel != null)
                    {
                        // API'den gelen JWT token'ını oku ve içindeki claims bilgilerini al.
                        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                        var token = handler.ReadJwtToken(tokenModel.Token);

                        // JWT token'ın içinde bulunan claims'leri (Id, Email, Role gibi) bir listeye dönüştürür ve
                        // Bu claims'ler içinde username, email gibi bilgileri oturumda sakla.
                        var claims = token.Claims.ToList();
                        claims.Add(new Claim("accessToken", tokenModel.Token));

                        // Kullanıcının yetkili bir oturum açmasını sağla.
                        var claimsIdentity = new ClaimsIdentity(
                            claims,
                            JwtBearerDefaults.AuthenticationScheme);

                        // Oturumun ne kadar süre geçerli olacağını ve kalıcı olup olmayacağını tanımla
                        var props = new AuthenticationProperties
                        {
                            ExpiresUtc = tokenModel.TokenExpiration,
                            IsPersistent = true, // Client bu token'i hatirlasin mi?
                        };

                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        // Kullanıcının giriş yaptığını uygulamaya bildir.
                        await HttpContext.SignInAsync(
                            JwtBearerDefaults.AuthenticationScheme,
                            claimsPrincipal,
                            props);

                        
                        // Kullanıcıyı HomeController içindeki Index action metoduna yönlendir
                        return RedirectToAction("HomePage", "Home"); 
                    }
                } else
                {
                    ModelState.AddModelError("", "Email or Password incorrect.");
                }
                return View(model);
            }
            return View(model);
        } 
    }

}
