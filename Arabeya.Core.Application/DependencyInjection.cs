using Arabeya.Core.Application.Abstraction.Sevices;
using Arabeya.Core.Application.Mapping;
using Arabeya.Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Arabeya.Core.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IServiceManager), typeof(ServiceManager));

            services.AddScoped(typeof(PictureUrlResolver));

           // services.AddAutoMapper(m => m.AddProfile(new MappingProfile()));
            services.AddAutoMapper(m=>m.AddProfile<MappingProfile>());



            return services;
        }
    }
}
