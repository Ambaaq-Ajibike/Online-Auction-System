using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Models
{
    public class Context:IdentityDbContext<ApplicationUser>
    {
        
        
        public Context(DbContextOptions options) : base(options)
        {

        }
    }
}
