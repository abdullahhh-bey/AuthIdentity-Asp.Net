using System.ComponentModel.DataAnnotations;

namespace UserAuthManagement.DTO
{
    public class CreateAdvisorDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Gender { get; set; } = string.Empty;
        [Required]
        public List<string> AdvisedCourses { get; set; } = new List<string>();
        [Required]
        public Int64 Salary { get; set; }
    }
}
