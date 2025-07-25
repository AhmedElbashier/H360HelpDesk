using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Domain.Helpers;
using webapi.Domain.Models;

namespace webapi.Domain.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EscalationMappingsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILog _log4netLogger;

        public EscalationMappingsController(AppDbContext context)
        {
            _context = context;
            _log4netLogger = LogManager.GetLogger("webapi.Domain.Controllers.EscalationMappingsController");

        }
        private TimeSpan? ParseTime(string? value)
        {
            if (TimeSpan.TryParse(value, out var result))
                return result;
            return null;
        }

        // GET: api/v1/EscalationMappings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EscalationMapping>>> GetAll()
        {
            return await _context.EscalationMappings.ToListAsync();
        }

        // GET: api/v1/EscalationMappings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EscalationMapping>> GetById(int id)
        {
            var mapping = await _context.EscalationMappings.FindAsync(id);
            if (mapping == null)
                return NotFound();

            return mapping;
        }

        [HttpPost]
        public async Task<ActionResult<EscalationMapping>> Create(EscalationMappingDto dto)
        {
            var mapping = new EscalationMapping
            {
                DepartmentID = dto.DepartmentID,
                CategoryID = dto.CategoryID,
                SubcategoryID = dto.SubcategoryID,
                PriorityID = dto.PriorityID,
                Level1Delay = ParseTime(dto.Level1Delay),
                Level2Delay = ParseTime(dto.Level2Delay),
                Level3Delay = ParseTime(dto.Level3Delay)
            };

            var level1Profile = await _context.EscalationLevels
            .Where(e => e.LevelID == dto.Level1LevelID)
            .Select(e => e.Profiles.OrderBy(p => p.ProfileID).FirstOrDefault())
            .FirstOrDefaultAsync();

            if (level1Profile == null)
                return BadRequest("Level 1 must have at least one profile.");

            mapping.Level1ProfileID = level1Profile.ProfileID;

            if (dto.Level2LevelID.HasValue)
            {
                var level2Profile = await _context.EscalationLevels
                    .Where(e => e.LevelID == dto.Level2LevelID)
                    .Select(e => e.Profiles.OrderBy(p => p.ProfileID).FirstOrDefault())
                    .FirstOrDefaultAsync();

                if (level2Profile != null)
                {
                    mapping.Level2ProfileID = level2Profile.ProfileID;
                }
                else
                {
                    _log4netLogger.Warn($"Level 2 selected (LevelID: {dto.Level2LevelID}) but no profiles found. Skipping Level 2.");
                    // Do not assign Level2ProfileID, skip silently.
                }
            }


            if (dto.Level2LevelID.HasValue)
            {
                var level3Profile = await _context.EscalationLevels
                    .Where(e => e.LevelID == dto.Level2LevelID)
                    .Select(e => e.Profiles.OrderBy(p => p.ProfileID).FirstOrDefault())
                    .FirstOrDefaultAsync();

                if (level3Profile != null)
                {
                    mapping.Level3ProfileID = level3Profile.ProfileID;
                }
                else
                {
                    _log4netLogger.Warn($"Level 3 selected (LevelID: {dto.Level3LevelID}) but no profiles found. Skipping Level 3.");
                    // Do not assign Level2ProfileID, skip silently.
                }
            }




            _context.EscalationMappings.Add(mapping);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = mapping.MappingID }, mapping);
        }


        // PUT: api/v1/EscalationMappings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, EscalationMapping mapping)
        {
            if (id != mapping.MappingID)
                return BadRequest();

            _context.Entry(mapping).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/v1/EscalationMappings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var mapping = await _context.EscalationMappings.FindAsync(id);
            if (mapping == null)
                return NotFound();

            _context.EscalationMappings.Remove(mapping);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Optional: GET /resolve-escalation?caseType=X&dept=Y&subcat=Z&prio=P
        [HttpGet("resolve-escalation")]
        public async Task<ActionResult<EscalationMapping>> ResolveEscalation(
            [FromQuery] int categoryID,
            [FromQuery] int departmentId,
            [FromQuery] int subcategoryId,
            [FromQuery] int priorityId)
        {
            var mapping = await _context.EscalationMappings.FirstOrDefaultAsync(m =>
                m.CategoryID == categoryID &&
                m.DepartmentID == departmentId &&
                m.SubcategoryID == subcategoryId &&
                m.PriorityID == priorityId
            );

            if (mapping == null)
                return NotFound("No escalation mapping found for provided filters.");

            return mapping;
        }
    }
    public class EscalationMappingDto
    {
        public int DepartmentID { get; set; }
        public int CategoryID { get; set; }
        public int SubcategoryID { get; set; }
        public int PriorityID { get; set; }

        public int? Level1LevelID { get; set; }
        public int? Level2LevelID { get; set; }
        public int? Level3LevelID { get; set; }

        public string? Level1Delay { get; set; }
        public string? Level2Delay { get; set; }
        public string? Level3Delay { get; set; }
    }

}
