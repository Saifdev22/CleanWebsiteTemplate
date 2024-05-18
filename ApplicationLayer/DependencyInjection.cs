using Microsoft.Extensions.DependencyInjection;

namespace ApplicationLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection ApplicationLayerDI(this IServiceCollection services)
        {
            return services;
        }
    }
}
