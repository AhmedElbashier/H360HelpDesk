using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Domain.Models;
using webapi.Domain.Helpers;
using log4net;

namespace webapi.Domain.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EscalationProfileController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILog _logger;

        public EscalationProfileController(AppDbContext context)
        {
            _context = context;
            _logger = LogManager.GetLogger(typeof(EscalationProfileController));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EscalationProfile>>> GetProfiles()
        {
            return await _context.EscalationProfiles.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EscalationProfile>> GetProfile(int id)
        {
            var profile = await _context.EscalationProfiles.FindAsync(id);
            if (profile == null) return NotFound();
            return profile;
        }

        [HttpPost]
        public async Task<ActionResult<EscalationProfile>> CreateProfile(EscalationProfile profile)
        {
            _context.EscalationProfiles.Add(profile);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProfile), new { id = profile.ProfileID }, profile);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfile(int id, EscalationProfile profile)
        {
            if (id != profile.ProfileID)
                return BadRequest();

            _context.Entry(profile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.EscalationProfiles.Any(e => e.ProfileID == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfile(int id)
        {
            var profile = await _context.EscalationProfiles.FindAsync(id);
            if (profile == null) return NotFound();

            _context.EscalationProfiles.Remove(profile);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
