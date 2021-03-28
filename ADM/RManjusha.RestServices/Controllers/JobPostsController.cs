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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class JobPostsController : ControllerBase
    {
        private readonly RManjushaContext _context;

        public JobPostsController(RManjushaContext context)
        {
            _context = context;
        }

        // GET: api/JobPosts
        [HttpGet]
        public IEnumerable<JobPost> GetJobPosts()
        {
            var user = (EmployerProfile)HttpContext.Items["Account"];
            if (user != null)
            {
                return _context.JobPosts.Where(x => x.PostedByEmpId == user.EmpId);
            }
            return Enumerable.Empty<JobPost>();
        }

        // GET: api/JobPosts/5
        [HttpGet("{id}")]
        public IActionResult GetJobPost(int id)
        {
            var jobPost = _context.JobPosts.Where(x => x.PostedByEmpId == id);

            if (jobPost == null)
            {
                return NotFound();
            }

            return Ok(jobPost);
        }

        // PUT: api/JobPosts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutJobPost([FromBody] JobPost jobPost)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            var entity = _context.JobPosts.FirstOrDefault(x => x.JobPostingId == jobPost.JobPostingId);
            if (entity != null)
            {
                entity.JobPostType = jobPost.JobPostType;
                entity.JobPrimarySkill = jobPost.JobPrimarySkill;
                entity.JobSecondarySkill = jobPost.JobSecondarySkill;
                entity.JobTitle = jobPost.JobTitle;
                entity.MaxExp = jobPost.MaxExp;
                entity.MinExp = jobPost.MinExp;
                entity.JobLocationId = jobPost.JobLocationId;
                entity.JobDescription = jobPost.JobDescription;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
                return Ok(entity);
            }
            return NoContent();
        }

        // POST: api/JobPosts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<JobPost>> PostJobPost(JobPost jobPost)
        {
            _context.JobPosts.Add(jobPost);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJobPost", new { id = jobPost.JobPostingId }, jobPost);
        }

        // DELETE: api/JobPosts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobPost(int id)
        {
            var jobPost = await _context.JobPosts.FindAsync(id);
            if (jobPost == null)
            {
                return NotFound();
            }

            _context.JobPosts.Remove(jobPost);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool JobPostExists(int id)
        {
            return _context.JobPosts.Any(e => e.JobPostingId == id);
        }
    }
}
