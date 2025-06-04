using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using webapi.Domain.Helpers;
using webapi.Domain.Models;

namespace webapi.Domain.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class SMTPController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILog _log4netLogger;

        public SMTPController(AppDbContext context)
        {
            _context = context;
            _log4netLogger = LogManager.GetLogger(typeof(SMTPController));
        }
        [HttpPost("test")]
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

        [HttpPost]
        public IActionResult CreateSMTPServer([FromBody] SmtpSettings SmtpSettingsReq)
        {
            if (SmtpSettingsReq == null)
            {
                return BadRequest("Invalid SmtpSettings data.");
            }

            var existingSmtpSettings = _context.SmtpSettings.FirstOrDefault();

            if (existingSmtpSettings != null)
            {
                // Update the existing record with the new data
                existingSmtpSettings.Host = SmtpSettingsReq.Host;
                existingSmtpSettings.Port = SmtpSettingsReq.Port;
                existingSmtpSettings.Username = SmtpSettingsReq.Username;
                existingSmtpSettings.Password = SmtpSettingsReq.Password;
                existingSmtpSettings.UseSsl = SmtpSettingsReq.UseSsl;
                existingSmtpSettings.UseDefaultCredentials = SmtpSettingsReq.UseDefaultCredentials;
                existingSmtpSettings.FromAddress = SmtpSettingsReq.FromAddress;
                existingSmtpSettings.DisplayName = SmtpSettingsReq.DisplayName;

                _context.SmtpSettings.Update(existingSmtpSettings);
            }
            else
            {
                // Create a new record
                var newSmtpSettings = new SmtpSettings
                {
                    Host = SmtpSettingsReq.Host,
                    Port = SmtpSettingsReq.Port,
                    Username = SmtpSettingsReq.Username,
                    Password = SmtpSettingsReq.Password,
                    UseSsl = SmtpSettingsReq.UseSsl,
                    UseDefaultCredentials = SmtpSettingsReq.UseDefaultCredentials,
                    FromAddress = SmtpSettingsReq.FromAddress,
                    DisplayName = SmtpSettingsReq.DisplayName
                };

                _context.SmtpSettings.Add(newSmtpSettings);
            }

            _context.SaveChanges();

            return Ok(); // You can return a success status code (200 OK) here.
        }


        [HttpGet]
        public IActionResult GetSmtpSettings()
        {
            try
            {
                var SmtpSettings = _context.SmtpSettings.ToList();
                if (SmtpSettings == null)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("GET Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + NotFound());
                    return NotFound();
                }
                return Ok(SmtpSettings);
            }
            catch (Exception ex)
            {
                ClientInfo client = new ClientInfo();
 
                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("GET Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t ", ex);
                return BadRequest();

            }
        }

        [HttpGet("{Id}")]
        public ActionResult<SmtpSettings> GetSmtpSettings(string Id)
        {
            try
            {
                var SmtpSettings = _context.SmtpSettings.Find(Id);

                if (SmtpSettings == null)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("GET Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + NotFound());

                    return NotFound();
                }
                return Ok(SmtpSettings);
            }
            catch (Exception ex)
            {
                ClientInfo client = new ClientInfo();
 
                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("GET Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t ", ex);
                return BadRequest();

            }
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> PutSmtpSettings(int Id, SmtpSettings SmtpSettings)
        {
            if (Id != SmtpSettings.Id)
            {
                ClientInfo client = new ClientInfo();
 
                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("PUT Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + BadRequest());
                return BadRequest();
            }

            _context.Entry(SmtpSettings).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SmtpSettingsExists(Id))
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("PUT Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + NotFound());
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }


        [HttpDelete("{Id}")]
        public async Task<ActionResult<SmtpSettings>> DeleteSmtpSettings(int Id)
        {
            try
            {
                var SmtpSettings = await _context.SmtpSettings.FindAsync(Id);
                if (SmtpSettings == null)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("DELETE Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + NotFound());
                    return NotFound();
                }

                _context.SmtpSettings.Remove(SmtpSettings);
                await _context.SaveChangesAsync();
                return SmtpSettings;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ClientInfo client = new ClientInfo();
 
                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("DELETE Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t ", ex);
                return BadRequest();

            }
        }
        [HttpDelete("DeleteSelected")]
        public async Task<IActionResult> DeleteSelectedSmtpSettings([FromBody] List<string> ids)
        {
            try
            {
                if (ids == null || ids.Count == 0)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + NotFound());
                    return BadRequest();
                }
                var SmtpSettings = await _context.SmtpSettings.Where(x => ids.Contains(x.Id.ToString())).ToListAsync();
                if (SmtpSettings == null || SmtpSettings.Count == 0)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + NotFound());
                    return NotFound();
                }
                _context.SmtpSettings.RemoveRange(SmtpSettings);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                ClientInfo client = new ClientInfo();
 
                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("DELETE Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t ", ex);
                return BadRequest();
            }
        }
        private bool SmtpSettingsExists(int Id)
        {
            return _context.SmtpSettings.Any(e => e.Id == Id);
        }

    }
}
