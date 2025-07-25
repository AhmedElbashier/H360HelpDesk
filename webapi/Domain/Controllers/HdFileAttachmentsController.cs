using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using webapi.Domain.Helpers;
using webapi.Domain.Models;

namespace webapi.Domain.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class HdFileAttachmentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILog _log4netLogger;


        public HdFileAttachmentsController(AppDbContext context)
        {
            _context = context;
            _log4netLogger = LogManager.GetLogger("webapi.Domain.Controllers.HdFileAttachmentsController");
        }

        [HttpPost]
        public ActionResult<HdFileAttachments> UploadHdFileAttachments(HdFileAttachments HdFileAttachmentsReq)
        {
            if (HdFileAttachmentsReq == null)
            {
                return BadRequest("Invalid HdLevels data.");
            }

            var HdFileAttachments = new HdFileAttachments
            {
                FileName = HdFileAttachmentsReq.FileName,
                FileSize = HdFileAttachmentsReq.FileSize,
                FileHash = HdFileAttachmentsReq.FileHash,
                FileData = HdFileAttachmentsReq.FileData,
                TicketID = HdFileAttachmentsReq.TicketID,
                CommentID = HdFileAttachmentsReq.CommentID,
                UserID = HdFileAttachmentsReq.UserID,
            };
            _context.HdFileAttachments.Add(HdFileAttachments);
            this._context.SaveChanges();

            return CreatedAtAction(nameof(GetHdFileAttachments), new { id = HdFileAttachments.FileID }, HdFileAttachments);
        }

        [HttpGet("DownloadFile/{fileId}")]
        public IActionResult DownloadFile(int fileId)
        {
            var hdFileAttachment = _context.HdFileAttachments.SingleOrDefault(f => f.FileID == fileId);

            if (hdFileAttachment == null)
            {
                return NotFound("File not found");
            }

            // Check if the FileData is already in base64 format
            string base64Content = hdFileAttachment.FileData;

            // If it's not in base64 format, convert it
            if (!IsBase64String(base64Content))
            {
                // Convert the file data (assuming it's a string) to bytes and then to base64
                byte[] fileBytes = Encoding.UTF8.GetBytes(hdFileAttachment.FileData);
                base64Content = Convert.ToBase64String(fileBytes);
            }

            return Ok(base64Content);
        }

        // Helper method to check if a string is in base64 format
        private bool IsBase64String(string s)
        {
            try
            {
                byte[] data = Convert.FromBase64String(s);
                return Convert.ToBase64String(data) == s;
            }
            catch
            {
                return false;
            }
        }

        //[HttpPost]
        //public IActionResult CreateAttachments([FromBody] List<HdFileAttachments> attachments)
        //{
        //    if (attachments == null || attachments.Count == 0)
        //    {
        //        return BadRequest();
        //    }

        //    _context.HdFileAttachments.AddRange(attachments);
        //    _context.SaveChanges();

        //    return CreatedAtAction("GetAttachments", attachments);
        //}


        [HttpGet]
        public IActionResult GetCategories()
        {
            try
            {
                var Categories = _context.HdFileAttachments.ToList();
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

        [HttpGet("Ticket/{TicketID}")]
        public ActionResult<HdFileAttachments> GetHdFileAttachments(int TicketID)
        {
            try
            {
                var HdFileAttachments = _context.HdFileAttachments.Where(x => x.TicketID == TicketID).ToList();

                if (HdFileAttachments == null)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("GET Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + NotFound());

                    return NotFound();
                }
                return Ok(HdFileAttachments);
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
        public ActionResult<HdFileAttachments> GetHdFileAttachments(string Id)
        {
            try
            {
                var HdFileAttachments = _context.HdFileAttachments.Find(Id);

                if (HdFileAttachments == null)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("GET Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + NotFound());

                    return NotFound();
                }
                return Ok(HdFileAttachments);
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
        public async Task<IActionResult> PutHdFileAttachments(int Id, HdFileAttachments HdFileAttachments)
        {
            if (Id != HdFileAttachments.FileID)
            {
                ClientInfo client = new ClientInfo();
 
                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("PUT Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + BadRequest());
                return BadRequest();
            }

            _context.Entry(HdFileAttachments).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HdFileAttachmentsExists(Id))
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
        public async Task<ActionResult<HdFileAttachments>> DeleteHdFileAttachments(int Id)
        {
            try
            {
                var HdFileAttachments = await _context.HdFileAttachments.FindAsync(Id);
                if (HdFileAttachments == null)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("DELETE Request IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + NotFound());
                    return NotFound();
                }

                _context.HdFileAttachments.Remove(HdFileAttachments);
                await _context.SaveChangesAsync();
                return HdFileAttachments;
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
        public async Task<IActionResult> DeleteSelectedHdFileAttachments([FromBody] List<string> ids)
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
                var HdFileAttachments = await _context.HdFileAttachments.Where(x => ids.Contains(x.FileID.ToString())).ToListAsync();
                if (HdFileAttachments == null || HdFileAttachments.Count == 0)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + NotFound());
                    return NotFound();
                }
                _context.HdFileAttachments.RemoveRange(HdFileAttachments);
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
        private bool HdFileAttachmentsExists(int Id)
        {
            return _context.HdFileAttachments.Any(e => e.FileID == Id);
        }

    }
}
