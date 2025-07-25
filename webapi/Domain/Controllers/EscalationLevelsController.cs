using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Domain.Helpers;
using webapi.Domain.Models;

namespace webapi.Domain.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EscalationLevelsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILog _log4netLogger;

        public EscalationLevelsController(AppDbContext context)
        {
            _context = context;
            _log4netLogger = LogManager.GetLogger("webapi.Domain.Controllers.EscalationLevelsController");

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EscalationLevel>>> GetAll()
        {
            return await _context.EscalationLevels
                .Include(l => l.Profiles)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EscalationLevel>> GetById(int id)
        {
            var level = await _context.EscalationLevels
                .Include(l => l.Profiles)
                .FirstOrDefaultAsync(l => l.LevelID == id);

            if (level == null)
                return NotFound();

            return level;
        }

        [HttpPost]
        public async Task<ActionResult<EscalationLevel>> Create(EscalationLevel level)
        {
            _context.EscalationLevels.Add(level);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = level.LevelID }, level);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, EscalationLevel level)
        {
            if (id != level.LevelID)
                return BadRequest();

            _context.Entry(level).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var level = await _context.EscalationLevels.FindAsync(id);
            if (level == null)
                return NotFound();

            _context.EscalationLevels.Remove(level);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
