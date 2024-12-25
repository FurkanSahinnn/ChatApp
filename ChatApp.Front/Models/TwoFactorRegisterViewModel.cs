using System.ComponentModel.DataAnnotations;

namespace ChatApp.Front.Models
{
    public class TwoFactorRegisterViewModel
    {
        [Display(Name = "Your Verification Code")]
        [Required(ErrorMessage = "Verification Code is required.")]
        [StringLength(8, ErrorMessage = "Verification code must be at least 8 characters long.")]
        public string VerificationCode { get; set; }
        public RegisterModel registerModel { get; set; }
    }
}
