using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RManjusha.RestServices.Helpers;
using RManjusha.RestServices.Models;

namespace RManjusha.RestServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsSetsController : ControllerBase
    {
        private readonly RManjushaContext _context;

        public SkillsSetsController(RManjushaContext context)
        {
            _context = context;
        }

        // GET: api/SkillsSets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SkillsSet>>> GetSkillsSets()
        {
            return await _context.SkillsSets.ToListAsync();
        }

        // GET: api/SkillsSets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SkillsSet>> GetSkillsSet(short id)
        {
            var skillsSet = await _context.SkillsSets.FindAsync(id);

            if (skillsSet == null)
            {
                return NotFound();
            }

            return skillsSet;
        }

        // PUT: api/SkillsSets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSkillsSet(short id, SkillsSet skillsSet)
        {
            if (id != skillsSet.SkillSetId)
            {
                return BadRequest();
            }

            _context.Entry(skillsSet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SkillsSetExists(id))
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

        // POST: api/SkillsSets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SkillsSet>> PostSkillsSet(SkillsSet skillsSet)
        {
            try
            {
                _context.SkillsSets.Add(skillsSet);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetSkillsSet", new { id = skillsSet.SkillSetId }, skillsSet);
            }catch(Exception ex)
            {
                return BadRequest(StatusCode(
                       StatusCodes.Status500InternalServerError,
                       "Add skill Failed: " + ex.GetInnermostException().Message));
            }
        }

        // DELETE: api/SkillsSets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSkillsSet(short id)
        {
            var skillsSet = await _context.SkillsSets.FindAsync(id);
            if (skillsSet == null)
            {
                return NotFound();
            }

            _context.SkillsSets.Remove(skillsSet);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SkillsSetExists(short id)
        {
            return _context.SkillsSets.Any(e => e.SkillSetId == id);
        }
    }
}
