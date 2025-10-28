using Arabeya.Core.Domain.Contracts.Persistence.DbInitializer;

namespace Arabeya.APIs.Extensions
{
    public static class DatabaseInitializerExtension
    {
        public static async Task<WebApplication> DatabaseAsync(this WebApplication webApplication)
        {
            var scoped=  webApplication.Services.CreateAsyncScope();
            var context = scoped.ServiceProvider;
            var IDentityContext = context.GetRequiredService<IDbIdenitityInitializer>();

            var logger=context.GetRequiredService<ILoggerFactory>();

            try
            {
                await IDentityContext.InitializeAsync();
                await IDentityContext.SeedAsync();
            }
            catch (Exception ex)
            {
                var log = logger.CreateLogger<Program>();

                log.LogError(ex.Message, ex);
            }

            return webApplication;
        }
    }
}
