using System.ComponentModel.DataAnnotations;

namespace ChatApp.Front.Models
{
    public class TwoFactorRegisterViewModel
    {
        [Display(Name = "Enter Your Verification Code")]
        [Required(ErrorMessage = "Verification Code is required.")]
        [StringLength(6, ErrorMessage = "Verification code must be 6 characters long.")]
        public string VerificationCode { get; set; }
    }
}
