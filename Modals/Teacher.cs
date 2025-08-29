namespace UserAuthManagement.Modals
{
    public class Teacher
    {
        public int Id { get; set; }
        public required string Name { get; set; } = string.Empty;
        public required string Gender { get; set; } = string.Empty;
        public required List<string> Description { get; set; }
        public required string Email { get; set; } = string.Empty;
        public int Grade { get; set; }
        public required Int64 Salary { get; set; }

    }
}
