global using Shared.DTOs;
global using Shared.Responses;
global using DomainLayer;
using InfrastructureLayer.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using InfrastructureLayer.Data;

namespace InfrastructureLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection InfrastructureLayerDI(this IServiceCollection services, IConfiguration configuration)
        {
            //Default Database Connection
            var defaultConnection = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(defaultConnection ?? throw new InvalidOperationException("Database connection not found!"));
            });

            //Identity Database Connection
            var identityConnection = configuration.GetConnectionString("IdentityConnection");

            services.AddDbContext<IdentityContext>(options =>
            {
                options.UseSqlServer(identityConnection ?? throw new InvalidOperationException("Database connection not found!"));
            });

            //Identity Connection
            services.AddIdentityCore<ApplicationUser>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<IdentityContext>()
                    .AddSignInManager();

            return services;
        }
    }
}
