using booking_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;
using System.Net.Mail;

namespace booking_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        /// <summary>
        /// Skicka e-postmeddelande.
        /// </summary>
        /// <param name="emailModel">Modell för e-postmeddelande.</param>
        /// <returns>Ok om e-postmeddelandet skickades, annars felstatuskod med felmeddelande.</returns>
        [HttpPost("send")]
        [SwaggerOperation(Summary = "Skicka e-postmeddelande")]
        [SwaggerResponse(200, "E-postmeddelandet skickades framgångsrikt")]
        [SwaggerResponse(400, "Ogiltiga indata för e-postmeddelande")]
        [SwaggerResponse(500, "Internt serverfel")]
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
