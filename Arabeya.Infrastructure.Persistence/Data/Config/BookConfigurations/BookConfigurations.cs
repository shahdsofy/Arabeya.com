using Arabeya.Core.Domain.Entities.Books;
using Arabeya.Infrastructure.Persistence.Data.Config.BaseConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arabeya.Infrastructure.Persistence.Data.Config.BookConfigurations
{
    public class BookConfigurations:BaseAuditableEntityConfigurations<Book,int>
    {
        public override void Configure(EntityTypeBuilder<Book> builder)
        {
            base.Configure(builder);

            builder.HasOne(b => b.User)
                  .WithMany(u => u.Books)
                  .HasForeignKey(b => b.UserId);


            builder.HasOne(b => b.Car)
                  .WithMany(c => c.Books)
                  .HasForeignKey(b => b.CarId);

        }
    }
}
