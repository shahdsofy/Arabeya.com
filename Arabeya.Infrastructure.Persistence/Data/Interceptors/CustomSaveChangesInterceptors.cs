using Arabeya.Core.Application.Abstraction;
using Arabeya.Core.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Arabeya.Infrastructure.Persistence.Data.Interceptors
{
    internal class CustomSaveChangesInterceptors:SaveChangesInterceptor
    {
        private readonly ILoggedInUserService _loggedInUserService;

        public CustomSaveChangesInterceptors(ILoggedInUserService loggedInUserService)
        {
            _loggedInUserService = loggedInUserService;
        }
        public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            UpdateEnities(eventData.Context!);
            return base.SavedChanges(eventData, result);
        }
        public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            UpdateEnities(eventData.Context!);
            return base.SavedChangesAsync(eventData, result, cancellationToken);
        }

        private void UpdateEnities(DbContext context)
        {
            foreach(var entry in context.ChangeTracker.Entries<BaseAuditableEntity<int>>().Where(e=>e.State is EntityState.Added or EntityState.Modified)
            {
                if(entry.State==EntityState.Added)
                {
                    entry.Entity.CreatedOn=DateTime.UtcNow;
                    entry.Entity.CreatedBy = _loggedInUserService.UserId;
                }

                entry.Entity.ModifiedOn=DateTime.UtcNow;
                entry.Entity.ModifiedBy = _loggedInUserService.UserId;

            }
    }
}
