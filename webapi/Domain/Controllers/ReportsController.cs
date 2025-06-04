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
    public class ReportsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILog _log4netLogger;

        public ReportsController(AppDbContext context)
        {
            _context = context;
            _log4netLogger = LogManager.GetLogger(typeof(ReportsController));

        }

        [HttpPost("Agent")]
        public IActionResult GetAgentReports(AgentReport AgentReportReq)
        {
            var query = _context.HdTickets
                .Where(x => x.UserID == AgentReportReq.UserID &&
                            (AgentReportReq.StatusID == 0 || x.StatusID == AgentReportReq.StatusID) &&
                            (AgentReportReq.DepartmentID == 0 || x.DepartmentID == AgentReportReq.DepartmentID) &&
                            (AgentReportReq.CategoryID == 0 || x.CategoryID == AgentReportReq.CategoryID) &&
                            (AgentReportReq.LevelID == 0 || x.Priority == AgentReportReq.LevelID) &&
                            (AgentReportReq.startdate == default(DateTime) || x.StartDate >= AgentReportReq.startdate) &&
                            (x.StartDate <= AgentReportReq.endDate));
            var ticket = query.ToList();

            return Ok(ticket);
        }

        [HttpPost("BackOffice")]
        public IActionResult GetBackOfficeReports(AgentReport AgentReportReq)
        {
            var query = _context.HdTickets
                .Where(x => x.AssingedToUserID == AgentReportReq.UserID &&
                            (AgentReportReq.StatusID == 0 || x.StatusID == AgentReportReq.StatusID) &&
                            (AgentReportReq.DepartmentID == 0 || x.DepartmentID == AgentReportReq.DepartmentID) &&
                            (AgentReportReq.CategoryID == 0 || x.CategoryID == AgentReportReq.CategoryID) &&
                            (AgentReportReq.LevelID == 0 || x.Priority == AgentReportReq.LevelID) &&
                            (AgentReportReq.startdate == default(DateTime) || x.StartDate >= AgentReportReq.startdate) &&
                            (x.StartDate <= AgentReportReq.endDate));
            var ticket = query.ToList();

            return Ok(ticket);
        }

        [HttpPost("Supervisor")]
        public IActionResult GetSuperVisorReports(SupervisorReport SupervisorReportReq)
        {
            var query = _context.HdTickets
              .Where(x => (x.AssingedToUserID == SupervisorReportReq.AssignedToUserID || SupervisorReportReq.AssignedToUserID == 0) &&
                          (x.UserID == SupervisorReportReq.UserID || SupervisorReportReq.UserID == 0) &&
                          (SupervisorReportReq.StatusID == 0 || x.StatusID == SupervisorReportReq.StatusID) &&
                          (SupervisorReportReq.DepartmentID == 0 || x.DepartmentID == SupervisorReportReq.DepartmentID) &&
                          (SupervisorReportReq.CategoryID == 0 || x.CategoryID == SupervisorReportReq.CategoryID) &&
                          (SupervisorReportReq.LevelID == 0 || x.Priority == SupervisorReportReq.LevelID) &&
                          (SupervisorReportReq.startdate == default(DateTime) || x.StartDate >= SupervisorReportReq.startdate) &&
                          (x.StartDate <= SupervisorReportReq.endDate));
            var ticket = query.ToList();

            return Ok(ticket);
        }

    }

}