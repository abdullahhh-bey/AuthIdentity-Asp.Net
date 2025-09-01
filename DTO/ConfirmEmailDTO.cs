using System.ComponentModel.DataAnnotations;

namespace UserAuthManagement.DTO
{
    public class ConfirmEmailDTO
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Token { get; set; } = string.Empty;

    }
}
