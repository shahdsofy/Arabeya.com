using Arabeya.Core.Domain.Entities.Books;
using Arabeya.Core.Domain.Entities.Reviews;
using Microsoft.AspNetCore.Identity;

namespace Arabeya.Core.Domain.Entities.Identity
{
    public class ApplicationUser:IdentityUser
    {
        public required string DisplayName { get; set; }
        public string? DrivingLicence { get; set; }


        public virtual ICollection<Book>? Books { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
    }
}
