using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureLayer.Identity
{
    public class IdentityContext(DbContextOptions options) : IdentityDbContext<ApplicationUser>(options)
    {
    }
}
