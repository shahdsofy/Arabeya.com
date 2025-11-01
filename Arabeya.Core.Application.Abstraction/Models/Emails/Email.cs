namespace Arabeya.Core.Application.Abstraction.Models.Emails
{
    public class Email
    {
        public string To { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
    }
}
