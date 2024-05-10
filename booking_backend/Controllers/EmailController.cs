using booking_backend.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Mail;

namespace booking_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        [HttpPost("send")]
        public IActionResult SendEmail([FromBody] EmailModel emailModel)
        {

            try
            {
                var fromAddress = new MailAddress("your-email@example.com", "Your Name");
                var toAddress = new MailAddress("recipient@example.com", "Recipient Name");
                const string fromPassword = "your-email-password";
                const string subject = "Contact Form Submission";
                string body = $"Name: {emailModel.Name}\nEmail: {emailModel.Email}\n\nMessage:\n{emailModel.Message}";

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

                return Ok("E-postmeddelande skickat!");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
