using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Arabeya.Core.Application.Abstraction.Models.Auth
{
    public class RegisterDto
    {

        [Required]
        public required string DisplayName { get; set; }

        [Required]
        public required string UserName { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string PhoneNumber { get; set; }
        [Required]
        public required string Password { get; set; }

        public IFormFile? DrivingLicence { get; set; }

        public string? Role { get; set; }
    }
}
