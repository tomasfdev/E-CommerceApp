using Microsoft.AspNetCore.Identity;

namespace Core.Models.Identity
{
    public class AppUser : IdentityUser
    {
        public string? DisplayName { get; set; }
        public virtual Address? Address { get; set; }
    }
}
