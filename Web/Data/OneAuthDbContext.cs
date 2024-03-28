using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OneAuth.Web.Data.Entities.Identity;

namespace OneAuth.Web.Data
{
    public class OneAuthDbContext(DbContextOptions<OneAuthDbContext> options) : AppDbContext<string>(options)
    {
    }

    public class AppDbContext<TKey> : IdentityDbContext<User>
        where TKey : IEquatable<TKey>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable(nameof(User));
            modelBuilder.Entity<IdentityRole>().ToTable(nameof(Role));
            modelBuilder.Entity<IdentityUserClaim<TKey>>().ToTable(nameof(UserClaim));
            modelBuilder.Entity<IdentityUserRole<TKey>>().ToTable(nameof(UserRole));
            modelBuilder.Entity<IdentityUserLogin<TKey>>().ToTable(nameof(UserLogin));
            modelBuilder.Entity<IdentityRoleClaim<TKey>>().ToTable(nameof(RoleClaim));
            modelBuilder.Entity<IdentityUserToken<TKey>>().ToTable(nameof(UserToken));
        }
    }
}
