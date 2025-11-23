using Arabeya.Core.Domain.Entities.Reviews;
using Arabeya.Infrastructure.Persistence.Data.Config.BaseConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arabeya.Infrastructure.Persistence.Data.Config.ReviewConfigurations
{
    public class ReviewConfigurations: BaseAuditableEntityConfigurations<Review,int>
    {
        public override void Configure(EntityTypeBuilder<Review> builder)
        {
            base.Configure(builder);

           

            builder.HasOne(e => e.User)
                .WithOne();


            builder.HasOne(e => e.Car)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);



       }
    }
}
