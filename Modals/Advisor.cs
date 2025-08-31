namespace UserAuthManagement.Modals
{
    public class Advisor
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public List<string> AdvisedCourses { get; set; } = new List<string>();
        public Int64 Salary {  get; set; }

    }
}
