// ... (other using statements)

using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using webapi.Domain.Helpers;
using webapi.Domain.Models;

namespace webapi.Domain.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILog _log4netLogger;
        private readonly AppDbContext _context;

        public AuthController(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
            _log4netLogger = LogManager.GetLogger("webapi.Domain.Controllers.AuthController");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            // Authenticate the user

            HdUsers user = _context.HdUsers.FirstOrDefault(u => u.Username == loginModel.Username);

            if (user == null || !IsValIdPassword(user, loginModel.Password))
            {
                _log4netLogger.Warn("User un authorized \t Username:" + loginModel.Username + " \tDB Username:" + loginModel.Username);
                return Unauthorized();
            }
            var token = GenerateJwtToken(user);
            return Ok(new { Token = token, User = user });
        }
        private string GenerateJwtToken(HdUsers user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TqszqFDnFdoQXhbPSi1H4gGZFj853KNUwtXPBfunXxs="));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.User_Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            };
            var tokenOptions = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpirationInMinutes"])),
                signingCredentials: credentials
                );
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return token;
        }
        [HttpPost("hash")]
        public IActionResult HashAdmin()
        {
            var hash = HashPassword("admin");
            return Ok(hash);
        }
        private bool IsValIdPassword(HdUsers user, string password)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, user.Password);
            }
            catch (Exception ex)
            {
                _log4netLogger.Error("Hashing Password BCrypt Error \t ", ex);
                return false;
            }
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        public class LoginModel
        {

            public string Username { get; set; }


            public string Password { get; set; }

        }
    }
}
