using System;
using System.Net;
using System.Net.Mail;

public class EmailService
{
    public void SendEmail(string senderEmail, string senderName, string recipientEmail, string recipientName, string subject, string body)
    {
        try
        {
            var fromAddress = new MailAddress(senderEmail, senderName);
            var toAddress = new MailAddress(recipientEmail, recipientName);

            const string fromPassword = "your-email-password";

            var smtp = new SmtpClient
            {
                Host = "smtp.example.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
                Console.WriteLine("E-postmeddelande skickat!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ett fel uppstod: {ex.Message}");
        }
    }
}
