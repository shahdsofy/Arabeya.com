using Arabeya.Core.Domain.Entities.Books;
using Arabeya.Core.Domain.Entities.Common;
using Arabeya.Core.Domain.Entities.Identity;
using Arabeya.Core.Domain.Entities.Reviews;

namespace Arabeya.Core.Domain.Entities.Cars
{
    public class Car:BaseAuditableEntity<int>
    {
        public string Model { get; set; } = null!;
        public int Year { get; set; }
        public bool IsAvailable { get; set; }
        public string? CarImageUrl { get; set; }
        public double PricePerDay { get; set; }
        public required string UserId { get; set; } = null!;
        public virtual required ApplicationUser Owner { get; set; }

        public virtual ICollection<Book> Books { get; set; }=new HashSet<Book>();
        public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();


        public double Rating { get; set; }
        public DateTime AvailableDate { get; set; }
    }
}
