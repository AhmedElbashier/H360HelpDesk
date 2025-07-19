using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using webapi.Domain.Helpers;
using webapi.Domain.Models;
using webapi.Domain.Services;

namespace webapi.Domain.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class EmailController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILog _log4netLogger;
        private readonly IEmailEscalationService _emailEscalationService;

        public EmailController(AppDbContext context, IEmailEscalationService emailEscalationService)
        {
            _emailEscalationService = emailEscalationService;
            _context = context;
            _log4netLogger = LogManager.GetLogger(typeof(SMTPController));
        }
        [HttpPost("send-test-email")]
        public async Task<IActionResult> TestEmail([FromBody] EmailRequest emailRequest)
        {
            var _smtpSettings = _context.SmtpSettings
                    .OrderByDescending(t => t.Id) // Assuming 'Id' is your auto-incrementing primary key
                    .FirstOrDefault();
            try
            {

                using (var client = new SmtpClient(_smtpSettings.Host))
                {
                    client.Port = _smtpSettings.Port;
                    if (!string.IsNullOrEmpty(_smtpSettings.Username) && !string.IsNullOrEmpty(_smtpSettings.Password))
                    {
                        client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                    }
                    client.EnableSsl = _smtpSettings.UseSsl;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_smtpSettings.FromAddress, _smtpSettings.DisplayName),
                        Subject = emailRequest.Subject,
                        Body = emailRequest.Body,
                        IsBodyHtml = true
                    };
                    foreach (var recipient in emailRequest.To)
                    {
                        mailMessage.To.Add(recipient);
                    }

                    await client.SendMailAsync(mailMessage);
                    return Ok("Email sent successfully.");
                }

            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return BadRequest($"Error sending email: {ex.Message}");
            }
        }

        [HttpPost("send-open-notification-email")]
        public async Task<IActionResult> OpenNotificationEmail([FromBody] EmailRequest emailRequest)
        {
            var _smtpSettings = _context.SmtpSettings
                    .OrderByDescending(t => t.Id) // Assuming 'Id' is your auto-incrementing primary key
                    .FirstOrDefault();
            try
            {

                using (var client = new SmtpClient(_smtpSettings.Host))
                {
                    client.Port = _smtpSettings.Port;
                    if (!string.IsNullOrEmpty(_smtpSettings.Username) && !string.IsNullOrEmpty(_smtpSettings.Password))
                    {
                        client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                    }
                    client.EnableSsl = _smtpSettings.UseSsl;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_smtpSettings.FromAddress, _smtpSettings.DisplayName),
                        Subject = emailRequest.Subject,
                        Body = emailRequest.Body,
                        IsBodyHtml = true
                    };
                    foreach (var recipient in emailRequest.To)
                    {
                        mailMessage.To.Add(recipient);
                    }

                    await client.SendMailAsync(mailMessage);
                    return Ok("Email sent successfully.");
                }

            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return BadRequest($"Error sending email: {ex.Message}");
            }
        }


        [HttpPost("send-close-notification-email")]
        public async Task<IActionResult> CloseNotificationEmail([FromBody] EmailRequest emailRequest)
        {
            var _smtpSettings = _context.SmtpSettings
                    .OrderByDescending(t => t.Id) // Assuming 'Id' is your auto-incrementing primary key
                    .FirstOrDefault();
            try
            {

                using (var client = new SmtpClient(_smtpSettings.Host))
                {
                    client.Port = _smtpSettings.Port;
                    if (!string.IsNullOrEmpty(_smtpSettings.Username) && !string.IsNullOrEmpty(_smtpSettings.Password))
                    {
                        client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                    }
                    client.EnableSsl = _smtpSettings.UseSsl;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_smtpSettings.FromAddress, _smtpSettings.DisplayName),
                        Subject = emailRequest.Subject,
                        Body = emailRequest.Body,
                        IsBodyHtml = true
                    };
                    foreach (var recipient in emailRequest.To)
                    {
                        mailMessage.To.Add(recipient);
                    }

                    await client.SendMailAsync(mailMessage);
                    return Ok("Email sent successfully.");
                }

            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return BadRequest($"Error sending email: {ex.Message}");
            }
        }

        [HttpGet("test-escalations")]
        public async Task<IActionResult> TestEscalations(CancellationToken cancellationToken)
        {
            await _emailEscalationService.ProcessEscalationsAsync(cancellationToken);
            return Ok("ProcessEscalationsAsync method has been executed.");
        }


    }
}