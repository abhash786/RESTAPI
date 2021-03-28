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
    public class LocationMastersController : ControllerBase
    {
        private readonly RManjushaContext _context;

        public LocationMastersController(RManjushaContext context)
        {
            _context = context;
        }

        // GET: api/LocationMasters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocationMaster>>> GetLocationMasters()
        {
            return await _context.LocationMasters.ToListAsync();
        }

        // GET: api/LocationMasters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LocationMaster>> GetLocationMaster(int id)
        {
            var locationMaster = await _context.LocationMasters.FindAsync(id);

            if (locationMaster == null)
            {
                return NotFound();
            }

            return locationMaster;
        }

        // PUT: api/LocationMasters/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocationMaster(int id, LocationMaster locationMaster)
        {
            if (id != locationMaster.LocationId)
            {
                return BadRequest();
            }

            _context.Entry(locationMaster).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationMasterExists(id))
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

        // POST: api/LocationMasters
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LocationMaster>> PostLocationMaster(LocationMaster locationMaster)
        {
            _context.LocationMasters.Add(locationMaster);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLocationMaster", new { id = locationMaster.LocationId }, locationMaster);
        }

        // DELETE: api/LocationMasters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocationMaster(int id)
        {
            var locationMaster = await _context.LocationMasters.FindAsync(id);
            if (locationMaster == null)
            {
                return NotFound();
            }

            _context.LocationMasters.Remove(locationMaster);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LocationMasterExists(int id)
        {
            return _context.LocationMasters.Any(e => e.LocationId == id);
        }
    }
}
