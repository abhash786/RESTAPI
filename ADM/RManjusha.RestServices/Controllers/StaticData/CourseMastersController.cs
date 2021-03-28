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
    public class CourseMastersController : ControllerBase
    {
        private readonly RManjushaContext _context;

        public CourseMastersController(RManjushaContext context)
        {
            _context = context;
        }

        // GET: api/CourseMasters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseMaster>>> GetCourseMasters()
        {
            return await _context.CourseMasters.ToListAsync();
        }

        // GET: api/CourseMasters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseMaster>> GetCourseMaster(short id)
        {
            var courseMaster = await _context.CourseMasters.FindAsync(id);

            if (courseMaster == null)
            {
                return NotFound();
            }

            return courseMaster;
        }

        // PUT: api/CourseMasters/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourseMaster(short id, CourseMaster courseMaster)
        {
            if (id != courseMaster.CourseId)
            {
                return BadRequest();
            }

            _context.Entry(courseMaster).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseMasterExists(id))
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

        // POST: api/CourseMasters
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CourseMaster>> PostCourseMaster(CourseMaster courseMaster)
        {
            _context.CourseMasters.Add(courseMaster);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourseMaster", new { id = courseMaster.CourseId }, courseMaster);
        }

        // DELETE: api/CourseMasters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourseMaster(short id)
        {
            var courseMaster = await _context.CourseMasters.FindAsync(id);
            if (courseMaster == null)
            {
                return NotFound();
            }

            _context.CourseMasters.Remove(courseMaster);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseMasterExists(short id)
        {
            return _context.CourseMasters.Any(e => e.CourseId == id);
        }
    }
}
