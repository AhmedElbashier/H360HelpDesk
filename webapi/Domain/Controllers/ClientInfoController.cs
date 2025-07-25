using log4net;
using Microsoft.AspNetCore.Mvc;
using webapi.Domain.Helpers;

namespace webapi.Domain.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ClientInfoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILog _log4netLogger;
        public ClientInfoController(AppDbContext context)
        {
            _context = context;
            _context = context;
            _log4netLogger = LogManager.GetLogger("webapi.Domain.Controllers.ClientInfoController");
        }

        [HttpGet]
        public IActionResult GetClientInfo()
        {
            try
            {
 
                var clientInfo = new
                {
                    IpAddress = HttpContext.Connection.RemoteIpAddress.ToString(),
                    Hostname = HttpContext.Request.Host.Host
                };
 
                return Ok(clientInfo);
            }
            catch (Exception ex)
            {
                _log4netLogger.Error("Client-Info-Controller error:", ex);
                return BadRequest();

            }
        }
    }
}
