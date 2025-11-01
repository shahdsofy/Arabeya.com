using Arabeya.Core.Application.Abstraction.Models.Emails;
using Arabeya.Core.Application.Abstraction.Sevices.Emails;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;


namespace Arabeya.Core.Application.Services.Email
{
    public class EmailService(IOptions<MailSettings>options) : IEmailService
    {

        private readonly MailSettings _mailSettings = options.Value;
        public async Task SendEmail(Abstraction.Models.Emails.Email email)
        {
            var mail = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Email),
                Subject = email.Subject,
            };

            mail.To.Add(MailboxAddress.Parse(email.To));
            mail.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Email));


            var builder = new BodyBuilder();
            builder.TextBody=email.Body;


            mail.Body = builder.ToMessageBody();


            using var smtp = new MailKit.Net.Smtp.SmtpClient();

           

            await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailSettings.Email, _mailSettings.Password);
            await smtp.SendAsync(mail);
            await smtp.DisconnectAsync(true);







        }
    }
}
