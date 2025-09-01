using System.ComponentModel.DataAnnotations;

namespace UserAuthManagement.DTO
{
    public class ResetPasswordDTO
    {
        [Required]
        public string Otp { get; set; } = string.Empty;
        [Required]
        public string Token { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string NewPassword { get; set; } = string.Empty;
    }
}
