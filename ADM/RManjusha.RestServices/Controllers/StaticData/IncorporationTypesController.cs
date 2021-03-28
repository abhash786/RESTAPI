using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RManjusha.RestServices.Models;

namespace RManjusha.RestServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncorporationTypesController : ControllerBase
    {
        private readonly RManjushaContext _context;

        public IncorporationTypesController(RManjushaContext context)
        {
            _context = context;
        }

        // GET: api/IncorporationTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IncorporationType>>> GetIncorporationTypes()
        {
            return await _context.IncorporationTypes.ToListAsync();
        }

        // GET: api/IncorporationTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IncorporationType>> GetIncorporationType(byte id)
        {
            var incorporationType = await _context.IncorporationTypes.FindAsync(id);

            if (incorporationType == null)
            {
                return NotFound();
            }

            return incorporationType;
        }

        // PUT: api/IncorporationTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIncorporationType(byte id, IncorporationType incorporationType)
        {
            if (id != incorporationType.IncId)
            {
                return BadRequest();
            }

            _context.Entry(incorporationType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IncorporationTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/IncorporationTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IncorporationType>> PostIncorporationType(IncorporationType incorporationType)
        {
            _context.IncorporationTypes.Add(incorporationType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIncorporationType", new { id = incorporationType.IncId }, incorporationType);
        }

        // DELETE: api/IncorporationTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncorporationType(byte id)
        {
            var incorporationType = await _context.IncorporationTypes.FindAsync(id);
            if (incorporationType == null)
            {
                return NotFound();
            }

            _context.IncorporationTypes.Remove(incorporationType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IncorporationTypeExists(byte id)
        {
            return _context.IncorporationTypes.Any(e => e.IncId == id);
        }
    }
}
