using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace ChatApp.API.Core.Application.Constants
{
    public static class PasswordHasher
    {
        public static string Hash(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        }

        public static bool Verify(string password, string passwordHashed)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHashed);
        }

    }
}
