using Arabeya.Core.Domain.Contracts.Persistence.DbInitializer;
using Microsoft.EntityFrameworkCore;

namespace Arabeya.Infrastructure.Persistence.Common
{
    internal abstract class DbInitializer(DbContext dbContext) : IDbInitializer
    {
        public async Task InitializeAsync()
        {
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
                await dbContext.Database.MigrateAsync();

        }

        public abstract Task SeedAsync();
      
    }
}
