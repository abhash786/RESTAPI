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
    public class SeekerTypesController : ControllerBase
    {
        private readonly RManjushaContext _context;

        public SeekerTypesController(RManjushaContext context)
        {
            _context = context;
        }

        // GET: api/SeekerTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeekerType>>> GetSeekerTypes()
        {
            return await _context.SeekerTypes.ToListAsync();
        }

        // GET: api/SeekerTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SeekerType>> GetSeekerType(byte id)
        {
            var seekerType = await _context.SeekerTypes.FindAsync(id);

            if (seekerType == null)
            {
                return NotFound();
            }

            return seekerType;
        }

        // PUT: api/SeekerTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeekerType(byte id, SeekerType seekerType)
        {
            if (id != seekerType.SkrTypeId)
            {
                return BadRequest();
            }

            _context.Entry(seekerType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeekerTypeExists(id))
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

        // POST: api/SeekerTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SeekerType>> PostSeekerType(SeekerType seekerType)
        {
            _context.SeekerTypes.Add(seekerType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSeekerType", new { id = seekerType.SkrTypeId }, seekerType);
        }

        // DELETE: api/SeekerTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeekerType(byte id)
        {
            var seekerType = await _context.SeekerTypes.FindAsync(id);
            if (seekerType == null)
            {
                return NotFound();
            }

            _context.SeekerTypes.Remove(seekerType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SeekerTypeExists(byte id)
        {
            return _context.SeekerTypes.Any(e => e.SkrTypeId == id);
        }
    }
}
