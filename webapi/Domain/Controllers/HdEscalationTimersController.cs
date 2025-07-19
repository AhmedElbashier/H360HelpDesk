using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Domain.Helpers;
using webapi.Domain.Models;

namespace webapi.Domain.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class HdEscalationTimersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILog _log4netLogger;

        public HdEscalationTimersController(AppDbContext context)
        {
            _context = context;
            _log4netLogger = LogManager.GetLogger(typeof(HdEscalationTimersController));
        }
        [HttpPost]
        public ActionResult<HdEscalationTimers> CreateHdEscalationTimer(HdEscalationTimers HdEscalationTimerReq)
        {
            if (HdEscalationTimerReq == null)
            {
                return BadRequest("Invalid HdEscalationTimer data.");
            }

            var HdEscalationTimer = new HdEscalationTimers
            {
                Id = HdEscalationTimerReq.Id,
                Hours = HdEscalationTimerReq.Hours,

            };
            _context.HdEscalationTimers.Add(HdEscalationTimer);
            this._context.SaveChanges();

            return CreatedAtAction(nameof(GetHdEscalationTimer), new { id = HdEscalationTimer.Id }, HdEscalationTimer);
        }

        [HttpGet]
        public IActionResult GetHdEscalationTimers()
        {
            try
            {
                var Requests = _context.HdEscalationTimers.ToList();
                if (Requests == null)
                {
                    ClientInfo client = new ClientInfo();

                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();

                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("GET Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + NotFound());
                    return NotFound();
                }
                return Ok(Requests);
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
        public ActionResult<HdEscalationTimers> GetHdEscalationTimer(string Id)
        {
            try
            {
                var HdEscalationTimer = _context.HdEscalationTimers.Find(Id);

                if (HdEscalationTimer == null)
                {
                    ClientInfo client = new ClientInfo();

                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();

                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("GET Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + NotFound());

                    return NotFound();
                }
                return Ok(HdEscalationTimer);
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
        public async Task<IActionResult> PutHdEscalationTimer(int Id, HdEscalationTimers HdEscalationTimer)
        {
            if (Id != HdEscalationTimer.Id)
            {
                ClientInfo client = new ClientInfo();

                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();

                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("PUT Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + BadRequest());
                return BadRequest();
            }

            _context.Entry(HdEscalationTimer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HdEscalationTimerExists(Id))
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
        public async Task<ActionResult<HdEscalationTimers>> DeleteHdEscalationTimer(int Id)
        {
            try
            {
                var HdEscalationTimer = await _context.HdEscalationTimers.FindAsync(Id);
                if (HdEscalationTimer == null)
                {
                    ClientInfo client = new ClientInfo();

                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();

                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("DELETE Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + NotFound());
                    return NotFound();
                }

                _context.HdEscalationTimers.Remove(HdEscalationTimer);
                await _context.SaveChangesAsync();
                return HdEscalationTimer;
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
      
        private bool HdEscalationTimerExists(int Id)
        {
            return _context.HdEscalationTimers.Any(e => e.Id == Id);
        }

    }
}
