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
    public class HdCompaniesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILog _log4netLogger;

        public HdCompaniesController(AppDbContext context)
        {
            _context = context;
            _log4netLogger = LogManager.GetLogger("webapi.Domain.Controllers.HdCompaniesController");
        }
        [HttpPost]
        public ActionResult<HdCompanies> CreatHdCompanies(HdCompanies HdCompaniesReq)
        {
            if (HdCompaniesReq == null)
            {
                return BadRequest("Invalid HdCompanies data.");
            }

            var HdCompanies = new HdCompanies
            {
                CompanyID = HdCompaniesReq.CompanyID,
                Description = HdCompaniesReq.Description,
            };
            _context.HdCompanies.Add(HdCompanies);
            this._context.SaveChanges();

            return CreatedAtAction(nameof(GetHdCompanies), new { id = HdCompanies.CompanyID }, HdCompanies);
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
            try
            {
                var Categories = _context.HdCompanies.ToList();
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
        public ActionResult<HdCompanies> GetHdCompanies(string Id)
        {
            try
            {
                var HdCompanies = _context.HdCompanies.Find(Id);

                if (HdCompanies == null)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("GET Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + NotFound());

                    return NotFound();
                }
                return Ok(HdCompanies);
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
        public async Task<IActionResult> PutHdCompanies(int Id, HdCompanies HdCompanies)
        {
            if (Id != HdCompanies.CompanyID)
            {
                ClientInfo client = new ClientInfo();
 
                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("PUT Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + BadRequest());
                return BadRequest();
            }

            _context.Entry(HdCompanies).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HdCompaniesExists(Id))
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
        public async Task<ActionResult<HdCompanies>> DeleteHdCompanies(int Id)
        {
            try
            {
                var HdCompanies = await _context.HdCompanies.FindAsync(Id);
                if (HdCompanies == null)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("DELETE Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + NotFound());
                    return NotFound();
                }

                _context.HdCompanies.Remove(HdCompanies);
                await _context.SaveChangesAsync();
                return HdCompanies;
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
        public async Task<IActionResult> DeleteSelectedHdCompanies([FromBody] List<string> ids)
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
                var HdCompanies = await _context.HdCompanies.Where(x => ids.Contains(x.CompanyID.ToString())).ToListAsync();
                if (HdCompanies == null || HdCompanies.Count == 0)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + NotFound());
                    return NotFound();
                }
                _context.HdCompanies.RemoveRange(HdCompanies);
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
        private bool HdCompaniesExists(int Id)
        {
            return _context.HdCompanies.Any(e => e.CompanyID == Id);
        }

    }
}
