using System.ComponentModel.DataAnnotations;

namespace UserAuthManagement.DTO
{
    public class StudentInfoDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Course { get; set; } = string.Empty;
    }
}
