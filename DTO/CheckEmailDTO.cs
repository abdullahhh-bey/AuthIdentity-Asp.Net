using System.ComponentModel.DataAnnotations;

namespace UserAuthManagement.DTO
{
    public class CheckEmailDTO
    {
        [Required]
        public string Email { get; set; } = string.Empty;  
    }
}
