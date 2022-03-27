using Microsoft.AspNetCore.Identity;

namespace DatingApp.Core.Model
{
    public class Role : IdentityRole<int>
    {
        // Foreign Keys (FK)
        public ICollection<UserRole> UserRoles { get; set; } // 1:M (FK) UserRole.RoleId
    }
}
