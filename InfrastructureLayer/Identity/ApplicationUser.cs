using Microsoft.AspNetCore.Identity;

namespace InfrastructureLayer.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
    }
}
