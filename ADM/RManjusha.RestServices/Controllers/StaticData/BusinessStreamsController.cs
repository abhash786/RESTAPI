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
    public class BusinessStreamsController : ControllerBase
    {
        private readonly RManjushaContext _context;

        public BusinessStreamsController(RManjushaContext context)
        {
            _context = context;
        }

        // GET: api/BusinessStreams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BusinessStream>>> GetBusinessStreams()
        {
            return await _context.BusinessStreams.ToListAsync();
        }

        // GET: api/BusinessStreams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessStream>> GetBusinessStream(short id)
        {
            var businessStream = await _context.BusinessStreams.FindAsync(id);

            if (businessStream == null)
            {
                return NotFound();
            }

            return businessStream;
        }

        // PUT: api/BusinessStreams/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBusinessStream(short id, BusinessStream businessStream)
        {
            if (id != businessStream.BusinessStreamId)
            {
                return BadRequest();
            }

            _context.Entry(businessStream).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusinessStreamExists(id))
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

        // POST: api/BusinessStreams
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BusinessStream>> PostBusinessStream(BusinessStream businessStream)
        {
            _context.BusinessStreams.Add(businessStream);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBusinessStream", new { id = businessStream.BusinessStreamId }, businessStream);
        }

        // DELETE: api/BusinessStreams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusinessStream(short id)
        {
            var businessStream = await _context.BusinessStreams.FindAsync(id);
            if (businessStream == null)
            {
                return NotFound();
            }

            _context.BusinessStreams.Remove(businessStream);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BusinessStreamExists(short id)
        {
            return _context.BusinessStreams.Any(e => e.BusinessStreamId == id);
        }
    }
}
