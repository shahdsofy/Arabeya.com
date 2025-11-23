using Arabeya.Core.Domain.Entities.Books;
using Arabeya.Core.Domain.Entities.Cars;
using Arabeya.Core.Domain.Entities.Reviews;
using Microsoft.EntityFrameworkCore;

namespace Arabeya.Infrastructure.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> dbContext) : base(dbContext)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StoreContext).Assembly);
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public DbSet<Book> Books { get; set; }
    }
}
