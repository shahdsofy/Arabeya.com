using Arabeya.Core.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arabeya.Infrastructure.Persistence.Data.Config.BaseConfigurations
{
    public class BaseAuditableEntityConfigurations<TEntity, TKey> : BaseEntityConfigurations<TEntity,TKey>
        where TEntity : BaseAuditableEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            base.Configure(builder);

            builder.Property(e=>e.CreatedOn)
                   .IsRequired();

            builder.Property(e => e.CreatedBy)
                     .IsRequired();

            builder.Property(e => e.ModifiedOn)
                     .IsRequired();

            builder.Property(e => e.ModifiedBy)
                .IsRequired();
        }
    }
}
