using Microsoft.AspNetCore.Identity;

namespace DatingApp.Core.Model
{
    public class UserRole : IdentityUserRole<int>
    {
        public User User { get; set; }
        public Role Role { get; set; }
    }
}
