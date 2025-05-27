using ChatApp.Front.Interfaces;
using ChatApp.Front.Services;
using ChatApp.Front.TwoFactorService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
// DI Container
builder.Services.Configure<TwoFactorOptions>(builder.Configuration.GetSection("TwoFactorOptions")); // Options Pattern for TwoFactorOptions class
builder.Services.AddScoped<EmailSenderService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddHttpClient(); // IHttpClientFactory servisi eklendi

var jwtSettings = builder.Configuration.GetSection("TokenOptions");
var secretKey = jwtSettings["SecurityKey"];
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddCookie(
JwtBearerDefaults.AuthenticationScheme, options => {
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.Cookie.SameSite = SameSiteMode.Strict; // Cookie'nin ilgili domain'de calismasini saglar.
    options.Cookie.HttpOnly = true; // Cookie'nin JS ile paylasilmasini engeller.
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;  // Gelen request'i Cookie'nin policy'si ile esler. (Http - http veya https - https)
    options.Cookie.Name = "JWTCookie";
});

// Session Middleware
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.Name = "RegisterSession";
});
var app = builder.Build();
// Static Files
app.UseStaticFiles();
// Routing
app.UseRouting();

// CQRS
// Authentication
app.UseAuthentication();
// Authorization
app.UseAuthorization();
app.UseSession();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{Controller}/{Action}/{id?}", // {id:int}
        defaults: new {Controller = "Account", Action = "Login" }
        );
    //endpoints.MapDefaultControllerRoute();
});

app.Run();
