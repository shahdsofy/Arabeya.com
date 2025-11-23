using Arabeya.Core.Domain.Entities.Cars;
using Arabeya.Core.Domain.Entities.Common;
using Arabeya.Core.Domain.Entities.Identity;

namespace Arabeya.Core.Domain.Entities.Reviews
{
    public class Review:BaseAuditableEntity<int>
    {
        public required string UserId { get; set; }

        public virtual required ApplicationUser User { get; set; } 
        public required int CarId { get; set; }
        public virtual required Car Car { get; set; }
        public string Comment { get; set; } = null!;
        public int Rating { get; set; }
    }
}
