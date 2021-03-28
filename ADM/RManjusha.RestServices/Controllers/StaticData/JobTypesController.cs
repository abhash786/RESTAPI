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
    public class JobTypesController : ControllerBase
    {
        private readonly RManjushaContext _context;

        public JobTypesController(RManjushaContext context)
        {
            _context = context;
        }

        // GET: api/JobTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobType>>> GetJobTypes()
        {
            return await _context.JobTypes.ToListAsync();
        }

        // GET: api/JobTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobType>> GetJobType(short id)
        {
            var jobType = await _context.JobTypes.FindAsync(id);

            if (jobType == null)
            {
                return NotFound();
            }

            return jobType;
        }

        // PUT: api/JobTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJobType(short id, JobType jobType)
        {
            if (id != jobType.JobTypeId)
            {
                return BadRequest();
            }

            _context.Entry(jobType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobTypeExists(id))
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

        // POST: api/JobTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<JobType>> PostJobType(JobType jobType)
        {
            _context.JobTypes.Add(jobType);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (JobTypeExists(jobType.JobTypeId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetJobType", new { id = jobType.JobTypeId }, jobType);
        }

        // DELETE: api/JobTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobType(short id)
        {
            var jobType = await _context.JobTypes.FindAsync(id);
            if (jobType == null)
            {
                return NotFound();
            }

            _context.JobTypes.Remove(jobType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool JobTypeExists(short id)
        {
            return _context.JobTypes.Any(e => e.JobTypeId == id);
        }
    }
}
