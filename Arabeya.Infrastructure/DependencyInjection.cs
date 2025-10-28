using Arabeya.Core.Domain.Contracts.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Arabeya.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services)
        {
            services.AddScoped(typeof(IAttachmentService), typeof(AttachmentService));
            return services;
        }
    }
}
