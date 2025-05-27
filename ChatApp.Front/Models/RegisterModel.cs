using System.ComponentModel.DataAnnotations;

namespace ChatApp.Front.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [RegularExpression(@"^.{8,}$", ErrorMessage = "Username must be at least 8 characters long.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Password must be at least 8 characters long, include uppercase, lowercase, and a number.")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Required(ErrorMessage = "Repeat Password is required.")]
        public string ConfirmPassword { get; set; }
    }
}
