using Arabeya.Core.Application.Abstraction.Models.Emails;
using Arabeya.Core.Application.Abstraction.Sevices;
using Arabeya.Core.Application.Abstraction.Sevices.Emails;
using Arabeya.Core.Application.Mapping;
using Arabeya.Core.Application.Services;
using Arabeya.Core.Application.Services.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arabeya.Core.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped(typeof(IServiceManager), typeof(ServiceManager));

            services.AddScoped(typeof(PictureUrlResolver));

           // services.AddAutoMapper(m => m.AddProfile(new MappingProfile()));
            services.AddAutoMapper(m=>m.AddProfile<MappingProfile>());

            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.AddTransient(typeof(IEmailService), typeof(EmailService));

            services.AddMemoryCache();

            return services;
        }
    }
}
