using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Mail;

[Route("api/[controller]")]
[ApiController]
public class EmailController : ControllerBase
{
    [HttpPost]
    public IActionResult SendEmail([FromBody] EmailModel model)
    {
        try
        {
            var fromAddress = new MailAddress("your-email@example.com", "Your Name");
            var toAddress = new MailAddress("recipient@example.com", "Recipient Name");
            const string fromPassword = "your-email-password";
            const string subject = "Subject";
            string body = $"From: {model.Email}\n\n{model.Message}";

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
            }

            return Ok("E-post skickat!");
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}

public class EmailModel
{
    public string? Email { get; set; }
    public string? Message { get; set; }
}
