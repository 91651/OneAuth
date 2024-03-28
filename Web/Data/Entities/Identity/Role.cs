using Microsoft.AspNetCore.Identity;

namespace OneAuth.Web.Data.Entities.Identity
{
    public class Role : IdentityRole
    {
        public Role()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
