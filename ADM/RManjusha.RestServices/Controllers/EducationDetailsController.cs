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
    public class EducationDetailsController : ControllerBase
    {
        private readonly RManjushaContext _context;

        public EducationDetailsController(RManjushaContext context)
        {
            _context = context;
        }

        // GET: api/EducationDetails/5
        [HttpGet]
        public ActionResult GetEducationDetail(string skrCode)
        {
            var educationDetail = _context.EducationDetails.Where(x => x.SkrCode == skrCode);

            if (educationDetail == null)
            {
                return Ok(new List<EducationDetail>());
            }

            return Ok(educationDetail);
        }

        // PUT: api/EducationDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEducationDetail(int id, EducationDetail educationDetail)
        {
            if (id != educationDetail.Id)
            {
                return BadRequest();
            }

            _context.Entry(educationDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EducationDetailExists(id))
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

        // POST: api/EducationDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public IActionResult PostEducationDetail(EducationDetail[] educationDetails)
        {
            foreach (var edu in educationDetails)
            {
                _context.EducationDetails.Add(edu);
                _context.SaveChanges();
            }

            return Ok(true);
        }

        // DELETE: api/EducationDetails/5
        [HttpDelete("{skrid}")]
        public IActionResult DeleteEducationDetail(int skrid)
        {
            var educationDetail = _context.EducationDetails.Where(x => x.SkrId == skrid).ToList();
            if (educationDetail == null || educationDetail.Count == 0)
            {
                return Ok();
            }
            foreach (var edu in educationDetail)
            {
                _context.EducationDetails.Remove(edu);
            }

            _context.SaveChanges();

            return Ok(true);
        }

        private bool EducationDetailExists(int id)
        {
            return _context.EducationDetails.Any(e => e.Id == id);
        }
    }
}
