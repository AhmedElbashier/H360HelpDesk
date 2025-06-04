using log4net;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Domain.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LogController : ControllerBase
    {
        private readonly ILog _log4netLogger;

        public LogController(ILogger<LogController> logger)
        {
            _log4netLogger = LogManager.GetLogger(typeof(LogController));
        }

        [HttpPost]
        public IActionResult Log([FromBody] LogModel logData)
        {
            _log4netLogger.Error(logData.Message);
            return Ok();
        }
    }

    public class LogModel
    {

        public string Level { get; set; }


        public string Message { get; set; }

    }
}
