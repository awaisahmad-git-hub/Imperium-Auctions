
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using ImperiumAuctions.Utility;
namespace ImperiumAuctions.Communication
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettingsOptions;

        public EmailSender(IOptions<EmailSettings> options)
        {
            _emailSettingsOptions = options.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(MailboxAddress.Parse(_emailSettingsOptions.SenderEmail));
            mimeMessage.To.Add(MailboxAddress.Parse(email));
            mimeMessage.Subject = subject;
            mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = htmlMessage
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_emailSettingsOptions.Host, _emailSettingsOptions.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_emailSettingsOptions.SenderEmail, _emailSettingsOptions.Password);
            await smtp.SendAsync(mimeMessage);
            await smtp.DisconnectAsync(true);
        } 
    }
}
