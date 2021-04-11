using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RManjusha.RestServices.Interceptors;
using RManjusha.RestServices.Models;

namespace RManjusha.RestServices.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExprerienceDetailsController : ControllerBase
    {
        private readonly RManjushaContext _context;

        public ExprerienceDetailsController(RManjushaContext context)
        {
            _context = context;
        }

        // GET: api/ExprerienceDetails/5
        [HttpGet]
        public ActionResult GetExprerienceDetail(string skrCode)
        {
            var exprerienceDetail = _context.ExperienceDetails.Where(x => x.SkrCode == skrCode);

            if (exprerienceDetail == null)
            {
                return Ok(new List<ExperienceDetail>());
            }

            return Ok(exprerienceDetail);
        }

        // PUT: api/ExprerienceDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExprerienceDetail(short id, ExperienceDetail exprerienceDetail)
        {
            if (id != exprerienceDetail.Id)
            {
                return BadRequest();
            }

            _context.Entry(exprerienceDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExprerienceDetailExists(id))
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

        // POST: api/ExprerienceDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public IActionResult PostExprerienceDetail(ExperienceDetail[] exprerienceDetails)
        {
            if (exprerienceDetails?.Length > 0)
            {
                foreach (var exp in exprerienceDetails)
                {
                    _context.Entry(exp).State = EntityState.Modified;

                    if (exp.LeavingDate == DateTime.MinValue)
                        exp.LeavingDate = null;
                    exp.JobCountry = "India";
                    _context.ExperienceDetails.Add(exp);
                    _context.SaveChanges();
                }
            }
            return Ok(true);
        }

        // DELETE: api/ExprerienceDetails/5
        [HttpDelete("{skrid}")]
        public IActionResult DeleteExprerienceDetail(int skrid)
        {
            var exprerienceDetail = _context.ExperienceDetails.Where(x => x.SkrId == skrid).ToList();
            if (exprerienceDetail == null || exprerienceDetail.Count == 0)
            {
                return Ok();
            }
            foreach (var exp in exprerienceDetail)
            {
                _context.ExperienceDetails.Remove(exp);
            }

            _context.SaveChanges();

            return Ok(true);
        }

        private bool ExprerienceDetailExists(short id)
        {
            return _context.ExperienceDetails.Any(e => e.Id == id);
        }
    }
}
