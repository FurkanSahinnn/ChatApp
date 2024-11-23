using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
// DI Container
builder.Services.AddHttpClient(); // IHttpClientFactory servisi eklendi
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddCookie(
    JwtBearerDefaults.AuthenticationScheme, options => {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.SameSite = SameSiteMode.Strict; // Cookie'nin ilgili domain'de calismasini saglar.
        options.Cookie.HttpOnly = true; // Cookie'nin JS ile paylasilmasini engeller.
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;  // Gelen request'i Cookie'nin policy'si ile esler. (Http - http veya https - https)
        options.Cookie.Name = "JWTCookie";
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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Account}/{action=Login}/{id?}"
        );
    //endpoints.MapDefaultControllerRoute();
});

app.Run();
