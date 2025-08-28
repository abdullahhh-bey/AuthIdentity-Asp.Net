using Microsoft.AspNetCore.Identity;

namespace UserAuthManagement.Modals
{
    public class User : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;

    }
}
