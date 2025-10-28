using Arabeya.Core.Domain.Contracts.Persistence.DbInitializer;
using Arabeya.Infrastructure.Persistence.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arabeya.Infrastructure.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services,IConfiguration configuration)
        {
            #region Identity
            services.AddDbContext<IdentityContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityContext"));
            });
            services.AddScoped(typeof(IDbIdenitityInitializer), typeof(IdentityDbInitializer));

            #endregion



            return services;
        }
    }
}
