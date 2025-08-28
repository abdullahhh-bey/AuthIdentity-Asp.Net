using System.ComponentModel.DataAnnotations;

namespace UserAuthManagement.DTO
{
    public class CreateStudentDTO
    {
        [Required]
        [StringLength(50, ErrorMessage = "Name is mandatory!")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Gender { get; set; } = string.Empty;

        [Required]
        public string Course { get; set; } = string.Empty;
    }
}
