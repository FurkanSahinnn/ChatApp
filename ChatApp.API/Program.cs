using ChatApp.API.Core.Application.Interfaces;
using ChatApp.API.Core.Application.Options;
using ChatApp.API.Core.Hubs;
using ChatApp.API.Persistence.Context;
using ChatApp.API.Persistence.Repositories;
using ChatApp.API.Persistence.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Reflection;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


var services = builder.Services;
// Add services to the container.
services.AddScoped<IUserService, UserService>();
services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.Configure<CustomTokenOptions>(builder.Configuration.GetSection("TokenOptions"));

services.AddMediatR(Assembly.GetExecutingAssembly());
services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(option =>
{
    var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<CustomTokenOptions>();
    option.RequireHttpsMetadata = false;

    option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidAudience = tokenOptions.Audience,
        ValidIssuer = tokenOptions.Issuer,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey)),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        RoleClaimType = ClaimTypes.Role // Role claim'ini kontrol et
    };

});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .SetIsOriginAllowed(origin => true)
              .AllowCredentials()
              .WithOrigins("http://localhost:5221", "https://localhost:7041", "https://localhost:7130");
    });
});
// Add DbContext
services.AddDbContext<AuthenticationContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chathub");



app.Run();
