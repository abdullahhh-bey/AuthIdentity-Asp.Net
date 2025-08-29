namespace UserAuthManagement.DTO
{
    public class UpdateTeacher
    {
        public  string Name { get; set; } = string.Empty;
        public  string Gender { get; set; } = string.Empty;
        public  List<string> Description { get; set; }
        public  string Email { get; set; } = string.Empty;
        public int Grade { get; set; }
        public  Int64 Salary { get; set; }
    }
}
