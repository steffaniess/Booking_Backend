using booking_backend.Services;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string content)
    {
        try
        {
            var fromAddress = new MailAddress(_emailSettings.Sender, _emailSettings.SenderName);
            var toAddress = new MailAddress(toEmail);

            using (var smtp = new SmtpClient
            {
                Host = _emailSettings.MailServer,
                Port = _emailSettings.MailPort,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, _emailSettings.Password)
            })
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = content,
                IsBodyHtml = true
            })
            {
                await smtp.SendMailAsync(message);
                Console.WriteLine("Thank you, your E-mail is send :)");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"A problem occured: {ex.Message}");
            throw; // Lägg till för att bubbla upp undantaget om du vill hantera det på högre nivå
        }
    }
}

// EmailSettings-klass för att matcha appsettings.json-konfigurationen
public class EmailSettings
{
    public string MailServer { get; set; }
    public int MailPort { get; set; }
    public string SenderName { get; set; }
    public string Sender { get; set; }
    public string Password { get; set; }
}
