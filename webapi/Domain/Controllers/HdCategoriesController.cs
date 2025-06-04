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
    public class HdCategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILog _log4netLogger;

        public HdCategoriesController(AppDbContext context)
        {
            _context = context;
            _log4netLogger = LogManager.GetLogger(typeof(HdCategoriesController));
        }
        [HttpPost]
        public ActionResult<HdCategories> CreatHdCategories(HdCategories HdCategoriesReq)
        {
            if (HdCategoriesReq == null)
            {
                return BadRequest("Invalid HdCategories data.");
            }

            var HdCategories = new HdCategories
            {
                CategoryID = HdCategoriesReq.CategoryID,
                Description = HdCategoriesReq.Description,
                DepartmentID = HdCategoriesReq.DepartmentID,

            };
            _context.HdCategories.Add(HdCategories);
            this._context.SaveChanges();

            return CreatedAtAction(nameof(GetHdCategories), new { id = HdCategories.CategoryID }, HdCategories);
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
            try
            {
                var Categories = _context.HdCategories.ToList();
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
        public ActionResult<HdCategories> GetHdCategories(string Id)
        {
            try
            {
                var HdCategories = _context.HdCategories.Find(Id);

                if (HdCategories == null)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("GET Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + NotFound());

                    return NotFound();
                }
                return Ok(HdCategories);
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
        public async Task<IActionResult> PutHdCategories(int Id, HdCategories HdCategories)
        {
            if (Id != HdCategories.CategoryID)
            {
                ClientInfo client = new ClientInfo();
 
                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("PUT Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + BadRequest());
                return BadRequest();
            }

            _context.Entry(HdCategories).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HdCategoriesExists(Id))
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
        public async Task<ActionResult<HdCategories>> DeleteHdCategories(int Id)
        {
            try
            {
                var HdCategories = await _context.HdCategories.FindAsync(Id);
                if (HdCategories == null)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("DELETE Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + NotFound());
                    return NotFound();
                }

                _context.HdCategories.Remove(HdCategories);
                await _context.SaveChangesAsync();
                return HdCategories;
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
        [HttpGet("bydepartment/{Id}")]
        public ActionResult<HdCategories> GetCategoryByDepartment(int Id)
        {
            try
            {
                var Category = _context.HdCategories.Where(x => x.DepartmentID == Id).ToList();

                if (Category == null)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("GET Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + NotFound());

                    return NotFound();
                }
                return Ok(Category);
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
        [HttpDelete("DeleteSelected")]
        public async Task<IActionResult> DeleteSelectedHdCategories([FromBody] List<string> ids)
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
                var HdCategories = await _context.HdCategories.Where(x => ids.Contains(x.CategoryID.ToString())).ToListAsync();
                if (HdCategories == null || HdCategories.Count == 0)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + NotFound());
                    return NotFound();
                }
                _context.HdCategories.RemoveRange(HdCategories);
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
        private bool HdCategoriesExists(int Id)
        {
            return _context.HdCategories.Any(e => e.CategoryID == Id);
        }

    }
}
