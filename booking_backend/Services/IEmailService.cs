using booking_backend.Helpers;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace booking_backend.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string content);
    }
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            var email = new MailMessage()
            {
                From = new MailAddress(_emailSettings.Sender, _emailSettings.SenderName),
                Subject = subject,
                Body = content,
                IsBodyHtml = true
            };
            email.To.Add(new MailAddress(toEmail));

            using var smtp = new SmtpClient(_emailSettings.MailServer, _emailSettings.MailPort)
            {
                Credentials = new NetworkCredential(_emailSettings.Sender, _emailSettings.Password),
                EnableSsl = true
            };
            await smtp.SendMailAsync(email);
        }
    }

}
