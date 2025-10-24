using Microsoft.Extensions.DependencyInjection;

namespace Arabeya.Infrastructure.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            return services;
        }
    }
}
