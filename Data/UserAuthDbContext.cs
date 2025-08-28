using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserAuthManagement.Modals;

namespace UserAuthManagement.Data
{
    public class UserAuthDbContext : IdentityDbContext<User>
    {
        public UserAuthDbContext(DbContextOptions<UserAuthDbContext> options) : base(options)
        {
           
        }
           
        public DbSet<User> Students { get; set; }

    }
}
