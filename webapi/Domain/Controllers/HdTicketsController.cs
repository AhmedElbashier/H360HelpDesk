using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Linq;
using webapi.Domain.Helpers;
using webapi.Domain.Models;
using webapi.Domain.Services;

namespace webapi.Domain.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class HdTicketsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILog _log4netLogger;
        private readonly SmsService _sms;
        private readonly EmailService _email;

        public HdTicketsController(AppDbContext context, SmsService sms, EmailService email)
        {
            _context = context;
            _log4netLogger = LogManager.GetLogger(typeof(HdTicketsController));
            _sms = sms;
            _email = email; 
        }
        [HttpPost]
        public ActionResult<HdTickets> UploadHdTickets([FromBody] HdTickets HdTicketsReq)
        {
            if (HdTicketsReq == null)
            {
                return BadRequest("InvalId HdTickets data.");
            }

            HdEscalation escalationLevel = _context.HdEscalation
                .FirstOrDefault(x => x.levelID == HdTicketsReq.Priority);


            DateTime? dueDate = null; // Initialize it as null
 
            dueDate = HdTicketsReq.StartDate.AddDays(escalationLevel.Days);
 

            var HdTikcet = new HdTickets
            {

                CustomerID = HdTicketsReq.CustomerID,
                Indice = HdTicketsReq.Indice,
                UserID = HdTicketsReq.UserID,
                CategoryID = HdTicketsReq.CategoryID,
                SubCategoryID = HdTicketsReq.SubCategoryID,
                DepartmentID = HdTicketsReq.DepartmentID,
                ChannelID = HdTicketsReq.ChannelID,
                AssingedToBackOfficeID = HdTicketsReq.AssingedToBackOfficeID,
                AssingedToUserID = HdTicketsReq.AssingedToUserID,
                Subject = HdTicketsReq.Subject,
                Body = HdTicketsReq.Body,
                StatusID = HdTicketsReq.StatusID,
                Priority = HdTicketsReq.Priority,
                EscalationLevel = escalationLevel != null ? escalationLevel.EscalationID : 0,
                UpdateByUser = HdTicketsReq.UpdateByUser,
                Email = HdTicketsReq.Email,
                EmailAlert = HdTicketsReq.EmailAlert,
                SMSAlert = HdTicketsReq.SMSAlert,
                RequestID = HdTicketsReq.RequestID,
                Flag = HdTicketsReq.Flag,
                Mobile = HdTicketsReq.Mobile,
                CustomerName = HdTicketsReq.CustomerName,
                CompanyID = HdTicketsReq.CompanyID,
                StartDate = HdTicketsReq.StartDate,
                ResolvedDate = HdTicketsReq.ResolvedDate,
                ClosedDate = HdTicketsReq.ClosedDate,
                DepartmentReply = HdTicketsReq.DepartmentReply,
                DueDate = dueDate,
            };
            _context.HdTickets.Add(HdTikcet);
            this._context.SaveChanges();
            var HdNewTickets = _context.HdTickets
                    .OrderByDescending(t => t.Id) // Assuming 'Id' is your auto-incrementing primary key
                    .FirstOrDefault();
            var mobile = HdNewTickets.Mobile;

            if (mobile.StartsWith("0"))
            {
                mobile = mobile.Substring(1);
            }

            if (!mobile.StartsWith("+966"))
            {
                mobile = "+966" + mobile;
            }

            var smsRequest = new SmsRequest
            {
                PhoneNumber = mobile,
            };
            var HdNewCategory = _context.HdCategories
            .Where(t => t.CategoryID == HdNewTickets.CategoryID) // Assuming 'Id' is your auto-incrementing primary key
            .FirstOrDefault();

            _sms.SendOpenSmsAsync(smsRequest, HdNewTickets.Id, HdNewCategory.Description, escalationLevel.Days.ToString());
            _email.SendOpenEmailAsync(HdNewTickets.Email, HdNewTickets.Id, HdNewCategory.Description, escalationLevel.Days.ToString());

            HdUsers HdUser = _context.HdUsers
                            .FirstOrDefault(x => x.User_Id == HdTicketsReq.UserID.ToString());
            if (HdUser == null)
            {
                return BadRequest("Error.");
            }
            else
            {
                var HdComments = new HdComments
                {

                    TicketID = HdNewTickets.Id,
                    CommentDate = HdNewTickets.StartDate,
                    UserID = HdNewTickets.UserID,
                    Body = HdUser.Lastname + " " + HdUser.Firstname + " created ticket ",
                    TicketFlag = true
                };
                var HdSMSComments = new HdComments
                {

                    TicketID = HdNewTickets.Id,
                    CommentDate = HdNewTickets.StartDate,
                    UserID = 0,
                    Body = "Ticket opened notification sent to the client by SMS",
                    TicketFlag = true
                };
                var HdEmailComments = new HdComments
                {

                    TicketID = HdNewTickets.Id,
                    CommentDate = HdNewTickets.StartDate,
                    UserID = 0,
                    Body = "Ticket opened notification sent to the client by Email",
                    TicketFlag = true
                };

                _context.HdComments.Add(HdComments);
                _context.HdComments.Add(HdSMSComments);
                _context.HdComments.Add(HdEmailComments);
                this._context.SaveChanges();


            }

            return CreatedAtAction(nameof(GetHdTickets), new { Id = HdTikcet.Id }, HdTikcet);
        }


        [HttpPost("UploadHdTicketsWithFile")]
        public ActionResult<HdTickets> UploadHdTicketsWithFile([FromForm] TicketWithFileUpload HdTicketsReq)
        {
            if (HdTicketsReq == null)
            {
                return BadRequest("InvalId HdTickets data.");
            }

            HdEscalation escalationLevel = _context.HdEscalation
                .FirstOrDefault(x => x.levelID == HdTicketsReq.Ticket.Priority);


            DateTime? dueDate = null; // Initialize it as null
 
            dueDate = HdTicketsReq.Ticket.StartDate.AddDays(escalationLevel.Days);
 

            var HdTikcet = new HdTickets
            {

                CustomerID = HdTicketsReq.Ticket.CustomerID,
                Indice = HdTicketsReq.Ticket.Indice,
                UserID = HdTicketsReq.Ticket.UserID,
                CategoryID = HdTicketsReq.Ticket.CategoryID,
                SubCategoryID = HdTicketsReq.Ticket.SubCategoryID,
                DepartmentID = HdTicketsReq.Ticket.DepartmentID,
                ChannelID = HdTicketsReq.Ticket.ChannelID,
                AssingedToBackOfficeID = HdTicketsReq.Ticket.AssingedToBackOfficeID,
                AssingedToUserID = HdTicketsReq.Ticket.AssingedToUserID,
                Subject = HdTicketsReq.Ticket.Subject,
                Body = HdTicketsReq.Ticket.Body,
                StatusID = HdTicketsReq.Ticket.StatusID,
                Priority = HdTicketsReq.Ticket.Priority,
                EscalationLevel = escalationLevel != null ? escalationLevel.EscalationID : 0,
                UpdateByUser = HdTicketsReq.Ticket.UpdateByUser,
                Email = HdTicketsReq.Ticket.Email,
                EmailAlert = HdTicketsReq.Ticket.EmailAlert,
                SMSAlert = HdTicketsReq.Ticket.SMSAlert,
                RequestID = HdTicketsReq.Ticket.RequestID,
                Flag = HdTicketsReq.Ticket.Flag,
                Mobile = HdTicketsReq.Ticket.Mobile,
                CustomerName = HdTicketsReq.Ticket.CustomerName,
                CompanyID = HdTicketsReq.Ticket.CompanyID,
                StartDate = HdTicketsReq.Ticket.StartDate,
                ResolvedDate = HdTicketsReq.Ticket.ResolvedDate,
                ClosedDate = HdTicketsReq.Ticket.ClosedDate,
                DueDate = dueDate,
            };
            _context.HdTickets.Add(HdTikcet);
            this._context.SaveChanges();

            var HdNewTickets = _context.HdTickets
            .OrderByDescending(t => t.Id) // Assuming 'Id' is your auto-incrementing primary key
            .FirstOrDefault(); 
            HdUsers HdUser = _context.HdUsers
                .FirstOrDefault(x => x.User_Id == HdTicketsReq.Ticket.UserID.ToString());

            var mobile = HdNewTickets.Mobile;

            if (mobile.StartsWith("0"))
            {
                mobile = mobile.Substring(1);
            }

            if (!mobile.StartsWith("+966"))
            {
                mobile = "+966" + mobile;
            }

            var smsRequest = new SmsRequest
            {
                PhoneNumber = mobile,
            };
            var HdNewCategory = _context.HdCategories
            .Where(t => t.CategoryID == HdNewTickets.CategoryID) // Assuming 'Id' is your auto-incrementing primary key
            .FirstOrDefault();

            _sms.SendOpenSmsAsync(smsRequest, HdNewTickets.Id, HdNewCategory.Description, escalationLevel.Days.ToString());
            _email.SendOpenEmailAsync(HdNewTickets.Email, HdNewTickets.Id, HdNewCategory.Description, escalationLevel.Days.ToString());


            if (HdUser == null)
            {
                return BadRequest("Error.");
            }
            else
            {
                var HdComments = new HdComments
                {

                    TicketID = HdNewTickets.Id,
                    CommentDate = HdNewTickets.StartDate,
                    UserID = HdNewTickets.UserID,
                    Body = HdUser.Lastname + " " + HdUser.Firstname + " created ticket ",
                    TicketFlag = true
                };
                var HdSMSComments = new HdComments
                {

                    TicketID = HdNewTickets.Id,
                    CommentDate = HdNewTickets.StartDate,
                    UserID = 0,
                    Body = "Ticket opened notification sent to the client by SMS",
                    TicketFlag = true
                };
                var HdEmailComments = new HdComments
                {

                    TicketID = HdNewTickets.Id,
                    CommentDate = HdNewTickets.StartDate,
                    UserID = 0,
                    Body = "Ticket opened notification sent to the client by Email",
                    TicketFlag = true
                };
                _context.HdComments.Add(HdComments);
                _context.HdComments.Add(HdSMSComments);
                _context.HdComments.Add(HdEmailComments);
                this._context.SaveChangesAsync();
            }

            var lastCreatedTicket = _context.HdTickets
                    .OrderByDescending(t => t.Id) // Assuming 'Id' is your auto-incrementing primary key
                    .FirstOrDefault();

            if (HdTicketsReq.File != null)
            {
 
                var fileEntity = new HdFileAttachments
                {
                    TicketID = lastCreatedTicket.Id,
                    CommentID = HdTicketsReq.File.CommentID,
                    UserID = HdTicketsReq.File.UserID,
                    FileName = HdTicketsReq.File.FileName,
                    FileHash = HdTicketsReq.File.FileHash,
                    FileSize = HdTicketsReq.File.FileSize,
                };
 
                _context.HdFileAttachments.Add(fileEntity);
                this._context.SaveChangesAsync();
            }


            return CreatedAtAction(nameof(GetHdTickets), new { Id = HdTikcet.Id }, HdTikcet);
        }

        [HttpGet]
        public IActionResult GetHdTickets()
        {
            try
            {
                var HdTickets = _context.HdTickets.ToList();
                if (HdTickets == null)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("GET Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + NotFound());
                    return NotFound();
                }
                return Ok(HdTickets);
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

        [HttpGet("lastCreatedTicket")]
        public ActionResult<HdTickets> GetLatHdTickets()
        {
            try
            {
                var HdTickets = _context.HdTickets
                    .OrderByDescending(t => t.Id) // Assuming 'Id' is your auto-incrementing primary key
                    .FirstOrDefault();

                if (HdTickets == null)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("GET Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + NotFound());

                    return NotFound();
                }
                return Ok(HdTickets.Id);
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
        public ActionResult<HdTickets> GetHdTickets(string Id)
        {
            try
            {
                var HdTickets = _context.HdTickets.Find(Id);

                if (HdTickets == null)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("GET Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + NotFound());

                    return NotFound();
                }
                return Ok(HdTickets);
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
        public async Task<IActionResult> PutHdTickets(int Id, HdTickets HdTickets)
        {
            if (Id != HdTickets.Id)
            {
                ClientInfo client = new ClientInfo();
 
                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("PUT Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + BadRequest());
                return BadRequest();
            }

            _context.Entry(HdTickets).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                HdUsers HdUser = _context.HdUsers
                .FirstOrDefault(x => x.User_Id == HdTickets.UserID.ToString());
                if (HdUser == null)
                {
                    return BadRequest("Error.");
                }
                else
                {
                    var HdComments = new HdComments
                    {

                        TicketID = HdTickets.Id,
                        CommentDate = HdTickets.StartDate,
                        UserID = HdTickets.UserID,
                        Body = HdUser.Lastname + " " + HdUser.Firstname + " updated the ticket details ",
                        TicketFlag = true
                    };

                    _context.HdComments.Add(HdComments);
                    this._context.SaveChanges();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HdTicketsExists(Id))
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
        [HttpPut("TicketTakeover")]
        public async Task<IActionResult> TicketTakeOver([FromBody] TicketTakeOverRequest request)
        {


            HdStatus HdStatus = _context.HdStatuses
                            .FirstOrDefault(x => x.Description == "In Progress");

            var HdTickets = await _context.HdTickets.FindAsync(request.Id);
 
            if (request.Id != HdTickets.Id)
            {
                ClientInfo client = new ClientInfo();
 
                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("PUT Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + BadRequest());
                return BadRequest();
            }
 
            HdTickets.AssingedToUserID = request.UserID;
            _context.Entry(HdTickets).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                DateTime updatetime = DateTime.Now;
                HdUsers HdUser = _context.HdUsers
                .FirstOrDefault(x => x.User_Id == HdTickets.UserID.ToString());
                if (HdUser == null)
                {
                    return BadRequest("Error.");
                }
                else
                {
                    var HdComments = new HdComments
                    {

                        TicketID = HdTickets.Id,
                        CommentDate = HdTickets.StartDate,
                        UserID = HdTickets.UserID,
                        Body = HdUser.Lastname + " " + HdUser.Firstname + " took over the ticket  ",
                        TicketFlag = true
                    };

                    _context.HdComments.Add(HdComments);
                    this._context.SaveChanges();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HdTicketsExists(request.Id))
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

        [HttpPut("TicketAccept")]
        public async Task<IActionResult> TicketAccept([FromBody] TicketTakeOverRequest request)
        {


            HdStatus HdStatus = _context.HdStatuses
                            .FirstOrDefault(x => x.Description == "In Progress");

            var HdTickets = await _context.HdTickets.FindAsync(request.Id);

            if (request.Id != HdTickets.Id)
            {
                ClientInfo client = new ClientInfo();

                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();

                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("PUT Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + BadRequest());
                return BadRequest();
            }

            HdTickets.AssingedToUserID = request.UserID;
            HdTickets.StatusID = HdStatus.StatusID;
            _context.Entry(HdTickets).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                DateTime updatetime = DateTime.Now;
                HdUsers HdUser = _context.HdUsers
                .FirstOrDefault(x => x.User_Id == HdTickets.UserID.ToString());
                if (HdUser == null)
                {
                    return BadRequest("Error.");
                }
                else
                {
                    var HdComments = new HdComments
                    {

                        TicketID = HdTickets.Id,
                        CommentDate = HdTickets.StartDate,
                        UserID = HdTickets.UserID,
                        Body = HdUser.Lastname + " " + HdUser.Firstname + " accepted & handled the ticket  ",
                        TicketFlag = true
                    };

                    _context.HdComments.Add(HdComments);
                    this._context.SaveChanges();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HdTicketsExists(request.Id))
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

        [HttpPut("BackOfficeTicketAccept")]
        public async Task<IActionResult> BackOfficeTicketAccept([FromBody] TicketTakeOverRequest request)
        {


            HdStatus HdStatus = _context.HdStatuses
                            .FirstOrDefault(x => x.Description == "In Progress");

            var HdTickets = await _context.HdTickets.FindAsync(request.Id);

            if (request.Id != HdTickets.Id)
            {
                ClientInfo client = new ClientInfo();

                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();

                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("PUT Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + BadRequest());
                return BadRequest();
            }

            HdTickets.AssingedToBackOfficeID = request.UserID;
            HdTickets.StatusID = HdStatus.StatusID;
            _context.Entry(HdTickets).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                DateTime updatetime = DateTime.Now;
                HdUsers HdUser = _context.HdUsers
                .FirstOrDefault(x => x.User_Id == HdTickets.UserID.ToString());
                if (HdUser == null)
                {
                    return BadRequest("Error.");
                }
                else
                {
                    var HdComments = new HdComments
                    {

                        TicketID = HdTickets.Id,
                        CommentDate = HdTickets.StartDate,
                        UserID = HdTickets.UserID,
                        Body = HdUser.Lastname + " " + HdUser.Firstname + " accepted & handled the ticket  ",
                        TicketFlag = true
                    };

                    _context.HdComments.Add(HdComments);
                    this._context.SaveChanges();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HdTicketsExists(request.Id))
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

        [HttpPut("AsignToOtherAgent")]
        public async Task<IActionResult> AsignToOtherAgent([FromBody] TicketTakeOverRequest request)
        {

            var HdTickets = await _context.HdTickets.FindAsync(request.Id);
 
            if (request.Id != HdTickets.Id)
            {
                ClientInfo client = new ClientInfo();
 
                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("PUT Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + BadRequest());
                return BadRequest();
            }
 
            HdTickets.AssingedToBackOfficeID = request.UserID;
            _context.Entry(HdTickets).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                DateTime updatetime = DateTime.Now;
                HdUsers HdUser = _context.HdUsers
                                .FirstOrDefault(x => x.User_Id == HdTickets.UserID.ToString());
                if (HdUser == null)
                {
                    return BadRequest("Error.");
                }
                else
                {
                    var HdComments = new HdComments
                    {

                        TicketID = HdTickets.Id,
                        CommentDate = HdTickets.StartDate,
                        UserID = HdTickets.UserID,
                        Body = "Ticket have been assigned to user: "+HdUser.Lastname + " " + HdUser.Firstname + " at ",
                        TicketFlag = true
                    };

                    _context.HdComments.Add(HdComments);
                    this._context.SaveChanges();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HdTicketsExists(request.Id))
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

        [HttpPut("TicketClose/{Id}")]
        public async Task<IActionResult> TicketClose(int Id, HdTickets HdTickets)
        {

            HdStatus HdStatus = _context.HdStatuses
                            .FirstOrDefault(x => x.Description == "Closed");

            if (Id != HdTickets.Id)
            {
                ClientInfo client = new ClientInfo();
 
                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("PUT Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + BadRequest());
                return BadRequest();
            }
 
            HdTickets.StatusID = HdStatus.StatusID;
            DateTime updatetime = DateTime.Now;
            HdTickets.ClosedDate = updatetime;
            _context.Entry(HdTickets).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                HdUsers HdUser = _context.HdUsers
                .FirstOrDefault(x => x.User_Id == HdTickets.UserID.ToString());
 

                if (HdUser == null)
                {
                    return BadRequest("Error.");
                }
                else
                {
                    var HdComments = new HdComments
                    {

                        TicketID = HdTickets.Id,
                        CommentDate = HdTickets.StartDate,
                        UserID = HdTickets.UserID,
                        Body = HdUser.Lastname + " " + HdUser.Firstname + " closed the ticket  ",
                        TicketFlag = true
                    };

                    _context.HdComments.Add(HdComments);
                    this._context.SaveChanges();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HdTicketsExists(Id))
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

        [HttpPut("TicketResolve/{Id}")]
        public async Task<IActionResult> TicketResolve(int Id, HdTickets HdTickets)
        {

            HdStatus HdStatus = _context.HdStatuses
                            .FirstOrDefault(x => x.Description == "Resolved");

            if (Id != HdTickets.Id)
            {
                ClientInfo client = new ClientInfo();
 
                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("PUT Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + BadRequest());
                return BadRequest();
            }

            DateTime resolvedTime = DateTime.Now;
            HdTickets.ResolvedDate = resolvedTime;
            HdTickets.StatusID = HdStatus.StatusID;
            
            _context.Entry(HdTickets).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                DateTime updatetime = DateTime.Now;
                HdUsers HdUser = _context.HdUsers
                .FirstOrDefault(x => x.User_Id == HdTickets.UserID.ToString());
                var mobile = HdTickets.Mobile;

                if (mobile.StartsWith("0"))
                {
                    mobile = mobile.Substring(1);
                }

                if (!mobile.StartsWith("+966"))
                {
                    mobile = "+966" + mobile;
                }

                var smsRequest = new SmsRequest
                {
                    PhoneNumber = mobile,
                };
                _sms.SendCloseSmsAsync(smsRequest, HdTickets.Id, HdTickets.DepartmentReply);
                _email.SendCloseEmailAsync(HdTickets.Email, HdTickets.Id, HdTickets.DepartmentReply);

                if (HdUser == null)
                {
                    return BadRequest("Error.");
                }
                else
                {
                    var HdComments = new HdComments
                    {

                        TicketID = HdTickets.Id,
                        CommentDate = HdTickets.StartDate,
                        UserID = HdTickets.UserID,
                        Body = HdUser.Lastname + " " + HdUser.Firstname + " resolved the ticket  ",
                        TicketFlag = true
                    };
                    var HdSMSComments = new HdComments
                    {

                        TicketID = HdTickets.Id,
                        CommentDate = HdTickets.StartDate,
                        UserID = 0,
                        Body = "Ticket closed notification sent to the client by SMS",
                        TicketFlag = true
                    };
                    var HdEmailComments = new HdComments
                    {

                        TicketID = HdTickets.Id,
                        CommentDate = HdTickets.StartDate,
                        UserID = 0,
                        Body = "Ticket closed notification sent to the client by Email",
                        TicketFlag = true
                    };
                    _context.HdComments.Add(HdComments);
                    _context.HdComments.Add(HdSMSComments);
                    _context.HdComments.Add(HdEmailComments);
                    this._context.SaveChanges();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HdTicketsExists(Id))
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




        [HttpPut("TicketReopen/{Id}")]
        public async Task<IActionResult> TicketReopen(int Id, HdTickets HdTickets)
        {

            HdStatus HdStatus = _context.HdStatuses
                            .FirstOrDefault(x => x.Description == "New");

            if (Id != HdTickets.Id)
            {
                ClientInfo client = new ClientInfo();
 
                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("PUT Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + BadRequest());
                return BadRequest();
            }
 
            HdTickets.StatusID = HdStatus.StatusID;
 
            _context.Entry(HdTickets).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                DateTime updatetime = DateTime.Now;
                HdUsers HdUser = _context.HdUsers
                                .FirstOrDefault(x => x.User_Id == HdTickets.UserID.ToString());
                if (HdUser == null)
                {
                    return BadRequest("Error.");
                }
                else
                {
                    var HdComments = new HdComments
                    {

                        TicketID = HdTickets.Id,
                        CommentDate = HdTickets.StartDate,
                        UserID = HdTickets.UserID,
                        Body = HdUser.Lastname + " " + HdUser.Firstname + " re-opened the ticket  ",
                        TicketFlag = true
                    };

                    _context.HdComments.Add(HdComments);
                    this._context.SaveChanges();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HdTicketsExists(Id))
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


        [HttpPut("TicketReopenInProgress/{Id}")]
        public async Task<IActionResult> TicketReopenInProgress(int Id, HdTickets HdTickets)
        {

            HdStatus HdStatus = _context.HdStatuses
                            .FirstOrDefault(x => x.Description == "In Progress");

            if (Id != HdTickets.Id)
            {
                ClientInfo client = new ClientInfo();

                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();

                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("PUT Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + BadRequest());
                return BadRequest();
            }

            HdTickets.StatusID = HdStatus.StatusID;

            _context.Entry(HdTickets).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                DateTime updatetime = DateTime.Now;
                HdUsers HdUser = _context.HdUsers
                                .FirstOrDefault(x => x.User_Id == HdTickets.UserID.ToString());
                if (HdUser == null)
                {
                    return BadRequest("Error.");
                }
                else
                {
                    var HdComments = new HdComments
                    {

                        TicketID = HdTickets.Id,
                        CommentDate = HdTickets.StartDate,
                        UserID = HdTickets.UserID,
                        Body = HdUser.Lastname + " " + HdUser.Firstname + " re-opened the ticket  ",
                        TicketFlag = true
                    };

                    _context.HdComments.Add(HdComments);
                    this._context.SaveChanges();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HdTicketsExists(Id))
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
        public async Task<ActionResult<HdTickets>> DeleteHdTickets(int Id)
        {
            try
            {
                var HdTickets = await _context.HdTickets.FindAsync(Id);
                if (HdTickets == null)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("DELETE Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + NotFound());
                    return NotFound();
                }

                _context.HdTickets.Remove(HdTickets);
                await _context.SaveChangesAsync();
                return HdTickets;
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
        public async Task<IActionResult> DeleteSelectedHdTickets([FromBody] List<string> Ids)
        {
            try
            {
                if (Ids == null || Ids.Count == 0)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + NotFound());
                    return BadRequest();
                }
                var HdTickets = await _context.HdTickets.Where(x => Ids.Equals(x.Id)).ToListAsync();
                if (HdTickets == null || HdTickets.Count == 0)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + NotFound());
                    return NotFound();
                }
                _context.HdTickets.RemoveRange(HdTickets);
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
        private bool HdTicketsExists(int Id)
        {
            return _context.HdTickets.Any(e => e.Id == Id);
        }

        [HttpGet("NewestOpened/{User_Id}")]
        public IActionResult GetNewestOpenedHdTicketsForUser(int User_Id)
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Where(x => x.UserID == User_Id)
                    .OrderByDescending(x => x.StartDate)
                    .Take(5)
                    .ToList();

                if (newestHdTicketsForUser == null || newestHdTicketsForUser.Count == 0)
                {
                    return NotFound();
                }

                return Ok(newestHdTicketsForUser);
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
        [HttpGet("UserHdTickets/{User_Id}")]
        public IActionResult GetUserHdTicketsForUser(int User_Id)
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Where(x => x.UserID == User_Id)
                    .ToList();

                if (newestHdTicketsForUser == null || newestHdTicketsForUser.Count == 0)
                {
                    return NotFound();
                }

                return Ok(newestHdTicketsForUser);
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


        [HttpGet("TotalCount/{User_Id}")]
        public IActionResult GetTotalHdTicketsCountForUser(int User_Id)
        {
            try
            {
                var totalHdTicketsCountForUser = _context.HdTickets
                    .Count(x => x.UserID == User_Id);

                return Ok(totalHdTicketsCountForUser);
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

        [HttpGet("TotalOpened/{User_Id}")]
        public IActionResult GetTotalOpenedHdTicketsForUser(int User_Id)
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Count(x => x.UserID == User_Id && x.StatusID == 2);

                return Ok(newestHdTicketsForUser);
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
        [HttpGet("TotalClosed/{User_Id}")]
        public IActionResult GetTotalClosedHdTicketsForUser(int User_Id)
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Count(x => x.UserID == User_Id && x.StatusID == 3);
                return Ok(newestHdTicketsForUser);
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
        [HttpGet("TotalResolved/{User_Id}")]
        public IActionResult GetTotalResolvedHdTicketsForUser(int User_Id)
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Count(x => x.UserID == User_Id && x.StatusID == 4);
                return Ok(newestHdTicketsForUser);
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
        [HttpGet("TotalNew/{User_Id}")]
        public IActionResult GetTotalNewHdTicketsForUser(int User_Id)
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Count(x => x.UserID == User_Id && x.StatusID == 1);
                return Ok(newestHdTicketsForUser);
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
        [HttpGet("TotalReopened/{User_Id}")]
        public IActionResult GetTotalReopenedHdTicketsForUser(int User_Id)
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Count(x => x.UserID == User_Id && x.StatusID == 5);
                return Ok(newestHdTicketsForUser);
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








        ////////////////////////////////////////////////////
        [HttpGet("UserOpened/{User_Id}")]
        public IActionResult GetOpenedHdTicketsForUser(int User_Id)
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Where(x => x.UserID == User_Id && x.StatusID == 2).ToList();

                return Ok(newestHdTicketsForUser);
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
        [HttpGet("UserClosed/{User_Id}")]
        public IActionResult GetClosedHdTicketsForUser(int User_Id)
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Where(x => x.UserID == User_Id && x.StatusID == 3).ToList();
                return Ok(newestHdTicketsForUser);
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
        [HttpGet("UserResolved/{User_Id}")]
        public IActionResult GetResolvedHdTicketsForUser(int User_Id)
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Where(x => x.UserID == User_Id && x.StatusID == 4).ToList();
                return Ok(newestHdTicketsForUser);
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
        [HttpGet("UserNew/{User_Id}")]
        public IActionResult GetNewHdTicketsForUser(int User_Id)
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Where(x => x.UserID == User_Id && x.StatusID == 1).ToList();
                return Ok(newestHdTicketsForUser);
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
        [HttpGet("UserReopened/{User_Id}")]
        public IActionResult GetReopenedHdTicketsForUser(int User_Id)
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Count(x => x.UserID == User_Id && x.StatusID == 5);
                return Ok(newestHdTicketsForUser);
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

        ////////////////////////////////////////////////////////////////////////////  BACK-OFFICE End-points //////////////////////////////////////////////////////////////////////////

        [HttpGet("BackOffice/DepartmentAll/{DepartmentID}")]
        public IActionResult GetBackOfficeTicketAllForDepartment(int DepartmentID)
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Where(x => x.DepartmentID == DepartmentID).ToList();

                return Ok(newestHdTicketsForUser);
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

        [HttpGet("BackOffice/DepartmentUnAnswered/{DepartmentID}")]
        public IActionResult GetBackOfficeTicketUnAnsweredForDepartment(int DepartmentID)
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Where(x => x.DepartmentID == DepartmentID && x.StatusID == 1).ToList();

                return Ok(newestHdTicketsForUser);
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


        [HttpGet("BackOffice/DepartmentInProgress/{DepartmentID}")]
        public IActionResult GetBackOfficeTicketInProgressForDepartment(int DepartmentID)
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Where(x => x.DepartmentID == DepartmentID && x.StatusID == 2).ToList();

                return Ok(newestHdTicketsForUser);
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

        [HttpGet("BackOffice/DepartmentClosed/{DepartmentID}")]
        public IActionResult GetBackOfficeTicketClosedForDepartment(int DepartmentID)
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Where(x => x.DepartmentID == DepartmentID && x.StatusID == 3).ToList();

                return Ok(newestHdTicketsForUser);
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

        [HttpGet("BackOffice/DepartmentResolved/{DepartmentID}")]
        public IActionResult GetBackOfficeTicketResolvedForDepartment(int DepartmentID)
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Where(x => x.DepartmentID == DepartmentID && x.StatusID == 4).ToList();

                return Ok(newestHdTicketsForUser);
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

        [HttpGet("BackOffice/DepartmentAssignedToUser/{AssingedToBackOfficeID}")]
        public IActionResult GetBackOfficeTicketAssignedToUser(int AssingedToBackOfficeID)
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Where(x => x.AssingedToBackOfficeID == AssingedToBackOfficeID).ToList();

                return Ok(newestHdTicketsForUser);
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

        //////// Total Count


        [HttpGet("BackOffice/DepartmentAllTotal/{DepartmentID}")]
        public IActionResult GetTotalBackOfficeTicketAllForDepartment(int DepartmentID)
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Count(x => x.DepartmentID == DepartmentID);

                return Ok(newestHdTicketsForUser);
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

        [HttpGet("BackOffice/DepartmentUnAnsweredTotal/{DepartmentID}")]
        public IActionResult GetTotalBackOfficeTicketUnAnsweredForDepartment(int DepartmentID)
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Count(x => x.DepartmentID == DepartmentID && x.StatusID == 1);

                return Ok(newestHdTicketsForUser);
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


        [HttpGet("BackOffice/DepartmentInProgressTotal/{DepartmentID}")]
        public IActionResult GetTotalBackOfficeTicketInProgressForDepartment(int DepartmentID)
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Count(x => x.DepartmentID == DepartmentID && x.StatusID == 2);

                return Ok(newestHdTicketsForUser);
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

        [HttpGet("BackOffice/DepartmentClosedTotal/{DepartmentID}")]
        public IActionResult GetTotalBackOfficeTicketClosedForDepartment(int DepartmentID)
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Count(x => x.DepartmentID == DepartmentID && x.StatusID == 3);

                return Ok(newestHdTicketsForUser);
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

        [HttpGet("BackOffice/DepartmentResolvedTotal/{DepartmentID}")]
        public IActionResult GetTotalBackOfficeTicketResolvedForDepartment(int DepartmentID)
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Count(x => x.DepartmentID == DepartmentID && x.StatusID == 4);

                return Ok(newestHdTicketsForUser);
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

        [HttpGet("BackOffice/DepartmentAssingedToBackOfficeTotal/{AssingedToBackOfficeID}")]
        public IActionResult GetTotalBackOfficeTicketAssignedToUser(int AssingedToBackOfficeID)
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Count(x => x.AssingedToBackOfficeID == AssingedToBackOfficeID && x.StatusID == 1 && x.StatusID == 2);

                return Ok(newestHdTicketsForUser);
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


        [HttpGet("Supervisor/Total")]
        public IActionResult GetTotalSupervisor()
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Count();

                return Ok(newestHdTicketsForUser);
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
        [HttpGet("Supervisor/TotalNew")]
        public IActionResult GetTotalNewSupervisor()
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Count(x => x.StatusID == 1);

                return Ok(newestHdTicketsForUser);
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
        [HttpGet("Supervisor/TotalOpen")]
        public IActionResult GetTotalOpenSupervisor()
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Count(x => x.StatusID == 2);

                return Ok(newestHdTicketsForUser);
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
        [HttpGet("Supervisor/TotalClosed")]
        public IActionResult GetTotalClosedSupervisor()
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Count(x => x.StatusID == 3);

                return Ok(newestHdTicketsForUser);
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
        [HttpGet("Supervisor/TotalResolved")]
        public IActionResult GetTotalResolvedSupervisor()
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Count(x => x.StatusID == 4);

                return Ok(newestHdTicketsForUser);
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

        [HttpGet("Supervisor/All")]
        public IActionResult GetAllSupervisor()
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .ToList();

                return Ok(newestHdTicketsForUser);
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
        [HttpGet("Supervisor/New")]
        public IActionResult GetNewSupervisor()
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Where(x => x.StatusID == 1).ToList();

                return Ok(newestHdTicketsForUser);
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
        [HttpGet("Supervisor/Open")]
        public IActionResult GetOpenSupervisor()
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Where(x => x.StatusID == 2).ToList();

                return Ok(newestHdTicketsForUser);
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
        [HttpGet("Supervisor/Closed")]
        public IActionResult GetClosedSupervisor()
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Where(x => x.StatusID == 3).ToList();

                return Ok(newestHdTicketsForUser);
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
        [HttpGet("Supervisor/Resolved")]
        public IActionResult GetResolvedSupervisor()
        {
            try
            {
                var newestHdTicketsForUser = _context.HdTickets
                    .Where(x => x.StatusID == 4).ToList();

                return Ok(newestHdTicketsForUser);
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
    }
}

