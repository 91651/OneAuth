using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OneAuth.Web.Data
{
    public class OneAuthDbContext(DbContextOptions<OneAuthDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
    }
}
