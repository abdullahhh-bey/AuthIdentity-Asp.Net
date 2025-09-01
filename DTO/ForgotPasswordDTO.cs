using System.ComponentModel.DataAnnotations;

namespace UserAuthManagement.DTO
{
    public class ForgotPasswordDTO
    {
        [Required]
        public string Email { get; set; } = string.Empty;
    }
}
