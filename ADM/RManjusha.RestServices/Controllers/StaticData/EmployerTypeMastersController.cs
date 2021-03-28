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
    public class EmployerTypeMastersController : ControllerBase
    {
        private readonly RManjushaContext _context;

        public EmployerTypeMastersController(RManjushaContext context)
        {
            _context = context;
        }

        // GET: api/EmployerTypeMasters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployerTypeMaster>>> GetEmployerTypeMasters()
        {
            return await _context.EmployerTypeMasters.ToListAsync();
        }

        // GET: api/EmployerTypeMasters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployerTypeMaster>> GetEmployerTypeMaster(byte id)
        {
            var employerTypeMaster = await _context.EmployerTypeMasters.FindAsync(id);

            if (employerTypeMaster == null)
            {
                return NotFound();
            }

            return employerTypeMaster;
        }

        // PUT: api/EmployerTypeMasters/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployerTypeMaster(byte id, EmployerTypeMaster employerTypeMaster)
        {
            if (id != employerTypeMaster.EmpTypeId)
            {
                return BadRequest();
            }

            _context.Entry(employerTypeMaster).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployerTypeMasterExists(id))
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

        // POST: api/EmployerTypeMasters
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmployerTypeMaster>> PostEmployerTypeMaster(EmployerTypeMaster employerTypeMaster)
        {
            _context.EmployerTypeMasters.Add(employerTypeMaster);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployerTypeMaster", new { id = employerTypeMaster.EmpTypeId }, employerTypeMaster);
        }

        // DELETE: api/EmployerTypeMasters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployerTypeMaster(byte id)
        {
            var employerTypeMaster = await _context.EmployerTypeMasters.FindAsync(id);
            if (employerTypeMaster == null)
            {
                return NotFound();
            }

            _context.EmployerTypeMasters.Remove(employerTypeMaster);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployerTypeMasterExists(byte id)
        {
            return _context.EmployerTypeMasters.Any(e => e.EmpTypeId == id);
        }
    }
}
