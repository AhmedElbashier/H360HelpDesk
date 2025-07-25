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
    public class HdUsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILog _log4netLogger;

        public HdUsersController(AppDbContext context)
        {
            _context = context;
            _log4netLogger = LogManager.GetLogger("webapi.Domain.Controllers.HdUsersController");

        }

        [HttpGet]
        public IActionResult GetHdUsers()
        {
            try
            {
                var HdUsers = _context.HdUsers.ToList();
                if (HdUsers == null)
                {
                    _log4netLogger.Error("No HdUsers records found");
                    return NotFound();
                }
                else
                {

                    return Ok(HdUsers);
                }
            }
            catch (Exception ex)
            {
                ClientInfo client = new ClientInfo();
 
                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t ", ex);
                return BadRequest();
            }
        }

        [HttpGet("{Id}")]
        public ActionResult<HdUsers> GetHdUser(string Id)
        {
            try
            {
                var HdUser = _context.HdUsers.Where(x => x.User_Id == (Id)).ToList();

                if (HdUser == null)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("GET Request: IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t No record with this HdUserID:+" + Id + " found");
                    return NotFound();
                }

                else
                { return Ok(HdUser); }
            }
            catch (Exception ex)
            {
                ClientInfo client = new ClientInfo();
 
                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("GET Request: IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t ", ex);
                return BadRequest();
            }

        }

        [HttpGet("ByName/{Username}")]
        public IActionResult GetHdUserByName(string Username)
        {
            try
            {
                var HdUser = _context.HdUsers.Where(x => x.Username == (Username)).ToList();
                if (HdUser == null)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("GET Request: IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t No record with this Username:+" + Username + " found");
                    return NotFound();
                }
                else
                {
                    return Ok(HdUser);
                }
            }
            catch (Exception ex)
            {
                ClientInfo client = new ClientInfo();
 
                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t ", ex);
                return BadRequest();
            }

        }
        [HttpGet("Agents")]
        public IActionResult GetAgentsUsers()
        {
            try
            {
                var HdUser = _context.HdUsers.Where(x => x.IsAgent == true).ToList();
                if (HdUser == null)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("GET Request: IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname);
                    return NotFound();
                }
                else
                {
                    return Ok(HdUser);
                }
            }
            catch (Exception ex)
            {
                ClientInfo client = new ClientInfo();
 
                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t ", ex);
                return BadRequest();
            }

        }

        [HttpGet("BackOffice")]
        public IActionResult GetBackOfficeUsers()
        {
            try
            {
                var HdUser = _context.HdUsers.Where(x => x.IsBackOffice == true).ToList();
                if (HdUser == null)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("GET Request: IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname);
                    return NotFound();
                }
                else
                {
                    return Ok(HdUser);
                }
            }
            catch (Exception ex)
            {
                ClientInfo client = new ClientInfo();
 
                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t ", ex);
                return BadRequest();
            }

        }

        [HttpGet("Supervisor")]
        public IActionResult GetSupervisorUsers()
        {
            try
            {
                var HdUser = _context.HdUsers.Where(x => x.IsSuperVisor == true).ToList();
                if (HdUser == null)
                {
                    ClientInfo client = new ClientInfo();

                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();

                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("GET Request: IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname);
                    return NotFound();
                }
                else
                {
                    return Ok(HdUser);
                }
            }
            catch (Exception ex)
            {
                ClientInfo client = new ClientInfo();

                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();

                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t ", ex);
                return BadRequest();
            }

        }
        [HttpGet("CountAllUsers")]
        public IActionResult GetCountAllUsers()
        {
            try
            {
                var HdUser = _context.HdUsers.Count();
                if (HdUser == null)
                {
                    ClientInfo client = new ClientInfo();

                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();

                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("GET Request: IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname);
                    return NotFound();
                }
                else
                {
                    return Ok(HdUser);
                }
            }
            catch (Exception ex)
            {
                ClientInfo client = new ClientInfo();

                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();

                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t ", ex);
                return BadRequest();
            }

        }
        [HttpGet("CountAdmins")]
        public IActionResult GetCountAdminsUsers()
        {
            try
            {
                var HdUser = _context.HdUsers.Where(x => x.IsAdministrator == true).Count();
                if (HdUser == null)
                {
                    ClientInfo client = new ClientInfo();

                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();

                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("GET Request: IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname);
                    return NotFound();
                }
                else
                {
                    return Ok(HdUser);
                }
            }
            catch (Exception ex)
            {
                ClientInfo client = new ClientInfo();

                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();

                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t ", ex);
                return BadRequest();
            }

        }
        [HttpGet("CountAgents")]
        public IActionResult GetCountAgentsUsers()
        {
            try
            {
                var HdUser = _context.HdUsers.Where(x => x.IsAgent == true).Count();
                if (HdUser == null)
                {
                    ClientInfo client = new ClientInfo();

                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();

                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("GET Request: IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname);
                    return NotFound();
                }
                else
                {
                    return Ok(HdUser);
                }
            }
            catch (Exception ex)
            {
                ClientInfo client = new ClientInfo();

                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();

                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t ", ex);
                return BadRequest();
            }

        }

        [HttpGet("CountBackOffices")]
        public IActionResult GetCountBackOfficeUsers()
        {
            try
            {
                var HdUser = _context.HdUsers.Where(x => x.IsBackOffice == true).Count();
                if (HdUser == null)
                {
                    ClientInfo client = new ClientInfo();

                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();

                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("GET Request: IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname);
                    return NotFound();
                }
                else
                {
                    return Ok(HdUser);
                }
            }
            catch (Exception ex)
            {
                ClientInfo client = new ClientInfo();

                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();

                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t ", ex);
                return BadRequest();
            }

        }

        [HttpGet("CountSupervisors")]
        public IActionResult GetCountSupervisorUsers()
        {
            try
            {
                var HdUser = _context.HdUsers.Where(x => x.IsSuperVisor == true).Count();
                if (HdUser == null)
                {
                    ClientInfo client = new ClientInfo();

                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();

                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("GET Request: IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname);
                    return NotFound();
                }
                else
                {
                    return Ok(HdUser);
                }
            }
            catch (Exception ex)
            {
                ClientInfo client = new ClientInfo();

                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();

                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t ", ex);
                return BadRequest();
            }

        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> PUTHdUser(int Id, HdUsers HdUser)
        {
            if (Id != HdUser.Id)
            {
                return BadRequest();
            }

            _context.Entry(HdUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                if (!HdUserExists(Id))
                {

                    _log4netLogger.Error(NotFound());
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            ClientInfo client = new ClientInfo();
 
            client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
            client.Hostname = HttpContext.Request.Host.Host;
            _log4netLogger.Error("PUT Request: IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t");
            return NoContent();
        }

        [HttpPut("reset-password/{Id}/{Password}")]
        public async Task<IActionResult> ResetHdUser(int Id, string Password)
        {

            var HdUser = _context.HdUsers.Find(Id);

            string hashedPassword = HashPassword(Password);
            HdUser.Password = hashedPassword; 
            _context.Entry(HdUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                if (!HdUserExists(Id))
                {

                    _log4netLogger.Error(NotFound());
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            ClientInfo client = new ClientInfo();

            client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();

            client.Hostname = HttpContext.Request.Host.Host;
            _log4netLogger.Error("PUT Request: IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t");
            return NoContent();
        }

        [HttpPost]
        public IActionResult CreateHdUser(HdUsers HdUserReq)
        {
            try
            {
                var licenseManager = new LicenseManager();
                int allUsersLimit = licenseManager.GetAllUsersLimit("license.lic");
                int adminUsersLimit = licenseManager.GetAdminUserLimit("license.lic");
                int agentUsersLimit = licenseManager.GetAgentUserLimit("license.lic");
                int supervisorUsersLimit = licenseManager.GetSupervisorUserLimit("license.lic");
                int backOfficeUsersLimit = licenseManager.GetBackOfficeUserLimit("license.lic");

                // Check if the user limit is valid
                if (allUsersLimit <= 0)
                {
                    return BadRequest("Invalid or missing license.");
                }
                int usersCount = _context.HdUsers.Count();
                int adminCount = _context.HdUsers.Count(x => x.IsAdministrator == true);
                int agentCount = _context.HdUsers.Count(x => x.IsAgent == true);
                int supervisorCount = _context.HdUsers.Count(x => x.IsSuperVisor == true);
                int backOfficeCount = _context.HdUsers.Count(x => x.IsBackOffice == true);
                if (HdUserReq.IsAdministrator==true)
                {
                    if (adminCount >= adminUsersLimit)
                    {
                        return BadRequest("User limit exceeded.");
                    }
                    else
                    {
                        string hashedPassword = HashPassword(HdUserReq.Password);
                        HdUserReq.Password = hashedPassword;
                        var HdUser = new HdUsers
                        {
                            User_Id = HdUserReq.User_Id,
                            Username = HdUserReq.Username,
                            Email = HdUserReq.Email,
                            Password = HdUserReq.Password,
                            IsAdministrator = HdUserReq.IsAdministrator,
                            IsSuperVisor = HdUserReq.IsSuperVisor,
                            IsAgent = HdUserReq.IsAgent,
                            IsBackOffice = HdUserReq.IsBackOffice,
                            Firstname = HdUserReq.Firstname,
                            Lastname = HdUserReq.Lastname,
                            Phone = HdUserReq.Phone,
                            Disabled = HdUserReq.Disabled,
                            LastSeen = HdUserReq.LastSeen,
                            IPAddress = HdUserReq.IPAddress,
                            HostName = HdUserReq.HostName,
                            Department_Id = HdUserReq.Department_Id,
                            Department_Name = HdUserReq.Department_Name,
                            LastPasswordChange = HdUserReq.LastPasswordChange,
                            LastLogoutDate = HdUserReq.LastLogoutDate,
                            Status = HdUserReq.Status,
                            Created_at = HdUserReq.Created_at,
                            Updated_at = HdUserReq.Updated_at,
                            Deleted = HdUserReq.Deleted,
                            IsDepartmentRestrictedSupervisor = HdUserReq.IsDepartmentRestrictedSupervisor

                        };
                        _context.HdUsers.Add(HdUser);
                        this._context.SaveChanges();
                        return Ok();
                    }
                }

                if (HdUserReq.IsAgent==true)
                {
                    if (agentCount >= agentUsersLimit)
                    {
                        return BadRequest("User limit exceeded.");
                    }
                    else
                    {
                        string hashedPassword = HashPassword(HdUserReq.Password);
                        HdUserReq.Password = hashedPassword;
                        var HdUser = new HdUsers
                        {
                            User_Id = HdUserReq.User_Id,
                            Username = HdUserReq.Username,
                            Email = HdUserReq.Email,
                            Password = HdUserReq.Password,
                            IsAdministrator = HdUserReq.IsAdministrator,
                            IsSuperVisor = HdUserReq.IsSuperVisor,
                            IsAgent = HdUserReq.IsAgent,
                            IsBackOffice = HdUserReq.IsBackOffice,
                            Firstname = HdUserReq.Firstname,
                            Lastname = HdUserReq.Lastname,
                            Phone = HdUserReq.Phone,
                            Disabled = HdUserReq.Disabled,
                            LastSeen = HdUserReq.LastSeen,
                            IPAddress = HdUserReq.IPAddress,
                            HostName = HdUserReq.HostName,
                            Department_Id = HdUserReq.Department_Id,
                            Department_Name = HdUserReq.Department_Name,
                            LastPasswordChange = HdUserReq.LastPasswordChange,
                            LastLogoutDate = HdUserReq.LastLogoutDate,
                            Status = HdUserReq.Status,
                            Created_at = HdUserReq.Created_at,
                            Updated_at = HdUserReq.Updated_at,
                            Deleted = HdUserReq.Deleted,
                            IsDepartmentRestrictedSupervisor = HdUserReq.IsDepartmentRestrictedSupervisor

                        };
                        _context.HdUsers.Add(HdUser);
                        this._context.SaveChanges();
                        return Ok();
                    }
                }

                if (HdUserReq.IsBackOffice == true)
                {
                    if (backOfficeCount >= backOfficeUsersLimit)
                    {
                        return BadRequest("User limit exceeded.");
                    }
                    else
                    {
                        string hashedPassword = HashPassword(HdUserReq.Password);
                        HdUserReq.Password = hashedPassword;
                        var HdUser = new HdUsers
                        {
                            User_Id = HdUserReq.User_Id,
                            Username = HdUserReq.Username,
                            Email = HdUserReq.Email,
                            Password = HdUserReq.Password,
                            IsAdministrator = HdUserReq.IsAdministrator,
                            IsSuperVisor = HdUserReq.IsSuperVisor,
                            IsAgent = HdUserReq.IsAgent,
                            IsBackOffice = HdUserReq.IsBackOffice,
                            Firstname = HdUserReq.Firstname,
                            Lastname = HdUserReq.Lastname,
                            Phone = HdUserReq.Phone,
                            Disabled = HdUserReq.Disabled,
                            LastSeen = HdUserReq.LastSeen,
                            IPAddress = HdUserReq.IPAddress,
                            HostName = HdUserReq.HostName,
                            Department_Id = HdUserReq.Department_Id,
                            Department_Name = HdUserReq.Department_Name,
                            LastPasswordChange = HdUserReq.LastPasswordChange,
                            LastLogoutDate = HdUserReq.LastLogoutDate,
                            Status = HdUserReq.Status,
                            Created_at = HdUserReq.Created_at,
                            Updated_at = HdUserReq.Updated_at,
                            Deleted = HdUserReq.Deleted,
                            IsDepartmentRestrictedSupervisor = HdUserReq.IsDepartmentRestrictedSupervisor
                        };
                        _context.HdUsers.Add(HdUser);
                        this._context.SaveChanges();
                        return Ok();
                    }
                }

                if (HdUserReq.IsSuperVisor == true)
                {
                    if (supervisorCount >= supervisorUsersLimit)
                    {
                        return BadRequest("User limit exceeded.");
                    }
                    else
                    {
                        string hashedPassword = HashPassword(HdUserReq.Password);
                        HdUserReq.Password = hashedPassword;
                        var HdUser = new HdUsers
                        {
                            User_Id = HdUserReq.User_Id,
                            Username = HdUserReq.Username,
                            Email = HdUserReq.Email,
                            Password = HdUserReq.Password,
                            IsAdministrator = HdUserReq.IsAdministrator,
                            IsSuperVisor = HdUserReq.IsSuperVisor,
                            IsAgent = HdUserReq.IsAgent,
                            IsBackOffice = HdUserReq.IsBackOffice,
                            Firstname = HdUserReq.Firstname,
                            Lastname = HdUserReq.Lastname,
                            Phone = HdUserReq.Phone,
                            Disabled = HdUserReq.Disabled,
                            LastSeen = HdUserReq.LastSeen,
                            IPAddress = HdUserReq.IPAddress,
                            HostName = HdUserReq.HostName,
                            Department_Id = HdUserReq.Department_Id,
                            Department_Name = HdUserReq.Department_Name,
                            LastPasswordChange = HdUserReq.LastPasswordChange,
                            LastLogoutDate = HdUserReq.LastLogoutDate,
                            Status = HdUserReq.Status,
                            Created_at = HdUserReq.Created_at,
                            Updated_at = HdUserReq.Updated_at,
                            Deleted = HdUserReq.Deleted,
                            IsDepartmentRestrictedSupervisor = HdUserReq.IsDepartmentRestrictedSupervisor
                        };
                        _context.HdUsers.Add(HdUser);
                        this._context.SaveChanges();
                        return Ok();
                    }
                }
                return BadRequest();


            }
            catch (Exception ex)
            {
                ClientInfo client = new ClientInfo();
 
                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("POST Request: IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + ex);
                return NotFound();
            }
        }


        [HttpDelete("{HdUserID}")]
        public async Task<ActionResult<HdUsers>> DeleteHdUser(int HdUserID)
        {
            try
            {

                var HdUser = await _context.HdUsers.FindAsync(HdUserID);
                if (HdUser == null)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("PUT Request: IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t No record with this HdUserID:+" + HdUserID + " found");
                    return NotFound();
                }

                _context.HdUsers.Remove(HdUser);
                await _context.SaveChangesAsync();

                return HdUser;
            }
            catch (Exception ex)
            {

                ClientInfo client = new ClientInfo();
 
                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("DELETE Request: IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + ex);
                return NotFound();
            }
        }

        // ... your other endpoints ...

        [HttpPut("{Id}/lastseen")]
        public async Task<IActionResult> UpdateLastSeen(string Id, [FromBody] UpdateLastSeenDto dto)
        {
            try
            {

                HdUsers HdUser = _context.HdUsers.FirstOrDefault(x => x.User_Id == Id);

                if (HdUser == null)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("PUT Request: IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + "HdUserID:" + Id + "\t" + NotFound());
                    return NotFound();
                }

                HdUser.LastSeen = dto.LastSeen;
                await _context.SaveChangesAsync();
                return NoContent();

            }
            catch (Exception ex)
            {

                ClientInfo client = new ClientInfo();
 
                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("PUT Request: IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + Id + ex);
                return BadRequest();
            }

        }
        [HttpPut("{Id}/lastLogoutDate")]
        public async Task<IActionResult> UpdateLastLogoutDate(string Id, [FromBody] UpdateLastLogoutDateDto dto)
        {
            try
            {

                HdUsers HdUser = _context.HdUsers.FirstOrDefault(x => x.User_Id == Id);


                if (HdUser == null)
                {
                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("PUT Request: IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + NotFound());
                    return NotFound();
                }

                HdUser.LastLogoutDate = dto.LastLogoutDate;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {

                ClientInfo client = new ClientInfo();
 
                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("DELETE Request: IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + ex);
                return BadRequest();
            }
        }


        [HttpPut("{HdUserID}/status")]
        public async Task<IActionResult> UpdateStatus(string HdUserID, [FromBody] UpdateStatusDto dto)
        {
            try
            {

                HdUsers HdUser = _context.HdUsers.FirstOrDefault(x => x.User_Id == HdUserID);


                if (HdUser == null)
                {
                    return NotFound();
                }

                HdUser.Status = dto.Status;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {

                ClientInfo client = new ClientInfo();
 
                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("PUT Request: IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + HdUserID + ex);
                return BadRequest();
            }
        }
        [HttpDelete("DeleteSelected")]
        public async Task<IActionResult> DeleteSelectedHdUser([FromBody] List<string> ids)
        {
            try
            {
                if (ids == null || ids.Count == 0)
                {

                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("DELETE Request: IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + "No Ids Selected");
                    return BadRequest();
                }

                var HdUser = await _context.HdUsers.Where(x => ids.Equals(x.User_Id)).ToListAsync();

                if (HdUser == null || HdUser.Count == 0)
                {

                    ClientInfo client = new ClientInfo();
 
                    client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                    client.Hostname = HttpContext.Request.Host.Host;
                    _log4netLogger.Error("DELETE Request: IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + "No matching records found for deletion.");
                    return NotFound();
                }

                _context.HdUsers.RemoveRange(HdUser);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                ClientInfo client = new ClientInfo();
 
                client.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
 
                client.Hostname = HttpContext.Request.Host.Host;
                _log4netLogger.Error("PUT Request: IP_Address: " + client.IpAddress + "\t Hostname: " + client.Hostname + "\t " + ex);
                return BadRequest();
            }
        }

        private bool HdUserExists(int Id)
        {
            return _context.HdUsers.Any(e => e.Id == Id);
        }
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
