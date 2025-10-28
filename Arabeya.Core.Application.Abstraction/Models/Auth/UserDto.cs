namespace Arabeya.Core.Application.Abstraction.Models.Auth
{
    public class UserDto
    {
        public required string Id { get; set; }
                
        public required string DisplayName { get; set; }
                
        public required string Email { get; set; }
                
        public string? DrivingLicence { get; set; }
                
        public required  string Token { get; set; }
    }
}
