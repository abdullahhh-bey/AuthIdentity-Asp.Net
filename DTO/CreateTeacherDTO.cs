using System.ComponentModel.DataAnnotations;

namespace UserAuthManagement.DTO
{
    public class CreateTeacherDTO
    {
        
        public required string Name { get; set; } = string.Empty;
        public required string Gender { get; set; } = string.Empty;
        public required List<string> Description { get; set; }

        [EmailAddress(ErrorMessage = "Enter valid Email")]
        public required string Email { get; set; } = string.Empty;
        public int Grade { get; set; }
        public required Int64 Salary { get; set; }
    }
}
