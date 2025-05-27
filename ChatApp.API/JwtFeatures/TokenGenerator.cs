using ChatApp.API.Core.Application.Dtos;
using ChatApp.API.Core.Application.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatApp.API.JwtFeatures
{
    public class TokenGenerator
    {
        public static TokenResponseDto CreateToken(IOptions<CustomTokenOptions> options, LoginUserResponseDto dto)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, dto.Id.ToString()));
            if (dto.UserName != null)
            {
                claims.Add(new Claim(ClaimTypes.Name, dto.UserName));
            }

            if (dto.Email != null)
            {
                claims.Add(new Claim(ClaimTypes.Email, dto.Email));
            }

            if (dto.Role != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, dto.Role));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SecurityKey!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            var tokenExpire = DateTime.UtcNow.AddMinutes(options.Value.AccessTokenExpiration);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                audience: options.Value.Audience,
                issuer: options.Value.Issuer,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: tokenExpire,
                signingCredentials: credentials);
            
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

            return new TokenResponseDto(token, tokenExpire);
        }
    }
}
