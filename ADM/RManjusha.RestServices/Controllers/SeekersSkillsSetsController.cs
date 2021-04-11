using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RManjusha.RestServices.Interceptors;
using RManjusha.RestServices.Models;
using RManjusha.RestServices.Securities;

namespace RManjusha.RestServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeekersSkillsSetsController : ControllerBase
    {
        private readonly RManjushaContext _context;

        public SeekersSkillsSetsController(RManjushaContext context)
        {
            _context = context;
        }

        // GET: api/SeekersSkillsSets
        [HttpGet]
        public ActionResult GetSeekersSkillsSets(string skrCode)
        {
            var skills = _context.SeekersSkillsSets.Where(x => x.SkrCode == skrCode).ToList();
            if (skills.Count > 0)
            {
                var sSkills = from s in skills
                              from id in _context.SkillsSets
                              where id.SkillSetId == s.SkillSetId
                              select new
                              {
                                  s.SkillSet.SkillSetName,
                                  s.SkillSet.SubSkilllSetName,
                                  s.SkillLevel,
                                  s.SkillSetId
                              };
                return Ok(sSkills);
            }
            return Ok(new List<SeekersSkillsSet>());
        }

        // GET: api/SeekersSkillsSets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SeekersSkillsSet>> GetSeekersSkillsSet(short id)
        {
            var seekersSkillsSet = await _context.SeekersSkillsSets.FindAsync(id);

            if (seekersSkillsSet == null)
            {
                return NotFound();
            }

            return seekersSkillsSet;
        }

        // PUT: api/SeekersSkillsSets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeekersSkillsSet(short id, SeekersSkillsSet seekersSkillsSet)
        {
            if (id != seekersSkillsSet.Id)
            {
                return BadRequest();
            }

            _context.Entry(seekersSkillsSet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeekersSkillsSetExists(id))
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

        // POST: api/SeekersSkillsSets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SeekersSkillsSet>> PostSeekersSkillsSet(SeekersSkillsSet seekersSkillsSet)
        {
            try
            {
                _context.SeekersSkillsSets.Add(seekersSkillsSet);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetSeekersSkillsSet", new { id = seekersSkillsSet.Id }, seekersSkillsSet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // DELETE: api/SeekersSkillsSets/5
        [HttpDelete("{skrid}")]
        public IActionResult DeleteSeekersSkillsSet(int skrid)
        {
            var seekersSkillsSet = _context.SeekersSkillsSets.Where(x => x.SkrId == skrid).ToList();
            if (seekersSkillsSet == null)
            {
                return Ok();
            }
            foreach (var skill in seekersSkillsSet)
            {
                _context.SeekersSkillsSets.Remove(skill);
            }

            _context.SaveChanges();

            return Ok(true);
        }

        private bool SeekersSkillsSetExists(short id)
        {
            return _context.SeekersSkillsSets.Any(e => e.Id == id);
        }
    }
}
