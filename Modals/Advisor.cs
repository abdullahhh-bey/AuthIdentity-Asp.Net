namespace UserAuthManagement.Modals
{
    public class Advisor
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string AdvisedCourses { get; set; } = string.Empty;
        public Int64 Salary {  get; set; }

    }
}
