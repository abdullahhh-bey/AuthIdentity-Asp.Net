namespace UserAuthManagement.DTO
{
    public class AdvisorInfoDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> AdvisedCourses { get; set; } = new List<string>();

    }
}
