using Arabeya.Core.Application.Abstraction.Models.Auth;
using Arabeya.Core.Application.Abstraction.Sevices.Auth;
using Arabeya.Core.Application.Services.Auth;
using Arabeya.Core.Domain.Entities.Identity;
using Arabeya.Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Identity;

namespace Arabeya.APIs.Extensions
{
    public static class IdentityExtension
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection services,IConfiguration configuration)
        {

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;

                options.SignIn.RequireConfirmedEmail = true;

            }).AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders();

            services.AddScoped(typeof(IAuthService),typeof(AuthService));


            services.AddScoped(typeof(Func<IAuthService>), serviceprovider =>
            {
                return () => serviceprovider.GetService<IAuthService>();
            });





            return services;
        }
    }
}
