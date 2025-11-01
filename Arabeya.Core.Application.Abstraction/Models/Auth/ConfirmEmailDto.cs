namespace Arabeya.Core.Application.Abstraction.Models.Auth
{
    public class ConfirmEmailDto
    {
        public string Email { get; set; } = null!;

        public string Token { get; set; }= null!;

      
    }
}
