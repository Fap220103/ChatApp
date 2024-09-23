using Microsoft.AspNetCore.Identity;

namespace ChatApp_Api.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
