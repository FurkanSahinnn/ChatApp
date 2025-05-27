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
using System.Reflection;
using ChatApp.Front.Utils;

namespace ChatApp.Front.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<TwoFactorOptions> _twoFactorOptions;
        private readonly EmailSenderService _emailSenderService;
        // TempData: Same action to action data transfering. 
        // RedirectToAction(): Different action to action data transferring
        public AccountController(IHttpClientFactory httpClientFactory, IOptions<TwoFactorOptions> twoFactorOptions, EmailSenderService emailSenderService)
        {
            _httpClientFactory = httpClientFactory;
            _twoFactorOptions = twoFactorOptions;
            _emailSenderService = emailSenderService;
        }

        public IActionResult TwoFactorRegister(RegisterModel model)
        {   
            if (_emailSenderService.TimeLeft(HttpContext) == 0)
            {
                return RedirectToAction("Login");
            }

            TempData.Put("registermodel", model); 

            // Mail gönderme işlemi burada yapılacak.
            ViewBag.timeLeft = _emailSenderService.TimeLeft(HttpContext); // Kalan Zamanı al.
            HttpContext.Session.SetString("codeVerification", _emailSenderService.Send(model.Email)); // Doğrulama kodunu gönder ve session'a kodu kaydet.
            // Formu görüntüler
            return View(new TwoFactorRegisterViewModel
            {
                VerificationCode = string.Empty
            });
        }
        

        [HttpPost]
        public async Task<IActionResult> TwoFactorRegister(TwoFactorRegisterViewModel model)
        {
            ViewBag.timeLeft = _emailSenderService.TimeLeft(HttpContext);
            if (model.VerificationCode == HttpContext.Session.GetString("codeVerification") && StringOperations.IsDigitsOnly(model.VerificationCode))
            {
                HttpContext.Session.Remove("currentTime");
                HttpContext.Session.Remove("codeVerification");

                var registerModel = TempData.Get<RegisterModel>("registermodel");

                // code verification yap.
                var client = _httpClientFactory.CreateClient();
                var content = new StringContent(JsonSerializer.Serialize(registerModel), Encoding.UTF8, "application/json");

                // Web API'ye POST isteği gönder
                var response = await client.PostAsync("http://localhost:5221/api/auth/register", content);

                if (response.IsSuccessStatusCode)
                {
                    // Başarılı kayıt sonrası giriş sayfasına yönlendir
                    ViewData["IsSuccess"] = true;
                    return RedirectToAction("Login", "Account");
                }
            }
            else
            {
                // Hata mesajını ekrana yazdır
                ModelState.AddModelError("", "An error occurred while registering the user.");
                ViewData["IsSuccess"] = false;
                return View(model);
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View(new RegisterModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            // Test123456
            
            if (ModelState.IsValid)
            {
                HttpContext.Session.Remove("currentTime");
                ViewData["IsSuccess"] = true;
                return RedirectToAction("TwoFactorRegister", model); 
            }
            ViewData["IsSuccess"] = false;
            return View(model);
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

                        
                        // JWT token'i cookie'ye kaydet
                        Response.Cookies.Append("JWTToken", tokenModel.Token, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict,
                            Expires = tokenModel.TokenExpiration
                        });
                        
                        
                        // Kullanıcı adı ve email al
                        var username = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                        var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                        var role = RoleParser.ParseStringToText(claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value); // 1 -> "admin", 2 -> "member"
                        if (role == "Member")
                        {
                            // Kullanıcıyı HomeController içindeki Index action metoduna yönlendir
                            ViewData["IsSuccess"] = true;
                            return RedirectToAction("HomePage", "Home");
                        } else if (role == "Admin")
                        {
                            ViewData["IsSuccess"] = true;
                            return RedirectToAction("HomePage", "Admin");
                        } else
                        {
                            ModelState.AddModelError("", "Unauthorized.");
                            ViewData["IsSuccess"] = false;
                        }
                    } else
                    {
                        ModelState.AddModelError("", "Invalid Token.");
                        ViewData["IsSuccess"] = false;
                    }
                } else
                {
                    ModelState.AddModelError("", "Email or Password incorrect.");
                    ViewData["IsSuccess"] = false;
                }
                return View(model);
                    
            }
            return View(model);
        } 
    }

}
