using Arabeya.Core.Domain.Entities.Cars;
using Arabeya.Core.Domain.Entities.Common;
using Arabeya.Infrastructure.Persistence.Data.Config.BaseConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arabeya.Infrastructure.Persistence.Data.Config.CarConfigurations
{
    public class CarConfigurations:BaseAuditableEntityConfigurations<Car,int>
    {
        public override void Configure(EntityTypeBuilder<Car> builder)
        {
            base.Configure(builder);

            builder.HasOne(c => c.Owner)
                   .WithMany() 
                   .HasForeignKey(c => c.UserId)
                   .OnDelete(DeleteBehavior.Restrict);


            builder.HasMany(c => c.Books)
                   .WithOne(b => b.Car)
                   .HasForeignKey(b => b.CarId);

            builder.HasMany(c => c.Reviews)
                   .WithOne(r => r.Car)
                   .HasForeignKey(r => r.CarId);

        }
    }
}
