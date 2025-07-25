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
    public class HdEscalationController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILog _log4netLogger;

        public HdEscalationController(AppDbContext context)
        {
            _context = context;
            _log4netLogger = LogManager.GetLogger("webapi.Domain.Controllers.HdEscalationController");
        }
        [HttpPost]
        public ActionResult<HdEscalation> CreatHdEscalation(HdEscalation HdEscalationReq)
        {
            if (HdEscalationReq == null)
            {
                return BadRequest("Invalid HdEscalation data.");
            }

            var HdEscalation = new HdEscalation
            {
                DepartmentID = HdEscalationReq.DepartmentID,
                levelID = HdEscalationReq.levelID,
                Days = HdEscalationReq.Days,
                Emails = HdEscalationReq.Emails,
            };
            _context.HdEscalation.Add(HdEscalation);
            this._context.SaveChanges();

            return CreatedAtAction(nameof(GetHdEscalation), new { id = HdEscalation.EscalationID }, HdEscalation);
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
            try
            {
                var Categories = _context.HdEscalation.ToList();
                if (Categories == null)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("GET Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + NotFound());
                    return NotFound();
                }
                return Ok(Categories);
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
        public ActionResult<HdEscalation> GetHdEscalation(string Id)
        {
            try
            {
                var HdEscalation = _context.HdEscalation.Find(Id);

                if (HdEscalation == null)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("GET Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + NotFound());

                    return NotFound();
                }
                return Ok(HdEscalation);
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
        public async Task<IActionResult> PutHdEscalation(int Id, HdEscalation HdEscalation)
        {
            if (Id != HdEscalation.EscalationID)
            {
                ClientInfo client = new ClientInfo();
 
                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("PUT Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + BadRequest());
                return BadRequest();
            }

            _context.Entry(HdEscalation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HdEscalationExists(Id))
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
        public async Task<ActionResult<HdEscalation>> DeleteHdEscalation(int Id)
        {
            try
            {
                var HdEscalation = await _context.HdEscalation.FindAsync(Id);
                if (HdEscalation == null)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("DELETE Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + NotFound());
                    return NotFound();
                }

                _context.HdEscalation.Remove(HdEscalation);
                await _context.SaveChangesAsync();
                return HdEscalation;
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
        public async Task<IActionResult> DeleteSelectedHdEscalation([FromBody] List<string> ids)
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
                var HdEscalation = await _context.HdEscalation.Where(x => ids.Contains(x.EscalationID.ToString())).ToListAsync();
                if (HdEscalation == null || HdEscalation.Count == 0)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + NotFound());
                    return NotFound();
                }
                _context.HdEscalation.RemoveRange(HdEscalation);
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
        private bool HdEscalationExists(int Id)
        {
            return _context.HdEscalation.Any(e => e.EscalationID == Id);
        }

    }
}
