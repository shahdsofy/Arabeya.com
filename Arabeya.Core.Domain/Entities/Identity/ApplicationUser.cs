using Microsoft.AspNetCore.Identity;

namespace Arabeya.Core.Domain.Entities.Identity
{
    public class ApplicationUser:IdentityUser
    {
        public required string DisplayName { get; set; }
        public string? DrivingLicence { get; set; }

    }
}
