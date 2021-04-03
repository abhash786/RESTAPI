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
    public class JobPostActivitiesController : ControllerBase
    {
        private readonly RManjushaContext _context;

        public JobPostActivitiesController(RManjushaContext context)
        {
            _context = context;
        }

        // GET: api/JobPostActivities/5
        [HttpGet("{id}/{isCompany}/{jobId}")]
        public IActionResult GetJobPostActivity(int id, bool isCompany, int jobId)
        {

            var jobs = new List<object>();
            if (isCompany)
            {
                List<JobPostActivity> jobPostActivity = null;
                if (jobId != 0)
                {
                    jobPostActivity = _context.JobPostActivities.Where(x => x.JobPostingId == jobId && x.JobPosting.PostedByEmp.EmpId == id)
                        .OrderBy(x => x.ApplicantApplyDate).ToList();
                }
                else
                    jobPostActivity = _context.JobPostActivities.Where(x => x.JobPosting.PostedByEmp.EmpId == id)
                            .OrderBy(x => x.ApplicantApplyDate).ToList();

                if (jobPostActivity == null)
                {
                    return NotFound();
                }
                foreach (JobPostActivity activity in jobPostActivity)
                {
                    var seeker = _context.SeekerProfiles.FirstOrDefault(x => x.SkrId == activity.SkrId);
                    if (seeker != null)
                    {
                        var j = _context.JobPosts.FirstOrDefault(x => x.JobPostingId == activity.JobPostingId);
                        var location = _context.LocationMasters.FirstOrDefault(x => x.LocationId == j.JobLocationId);
                        jobs.Add(new
                        {
                            ApplicationDate = activity.ApplicantApplyDate,
                            ApllicantName = $"{seeker.FirstName} {seeker.LastName}",
                            Email = seeker.Email,
                            Dob = seeker.Dob,
                            ContactNumber = seeker.ContactNum,
                            Languages = seeker.SpokenLanguage,
                            CommunicationAddress = seeker.CommAdd,
                            SkrId = seeker.SkrId,
                            JobId = activity.JobPostingId,
                            Job_Title = j.JobTitle,
                            Job_posting_date = j.JobCreatedDate,
                            Primary_Skill = j.JobPrimarySkill,
                            Secondary_Skil = j.JobSecondarySkill,
                            Location = location.City
                        });
                    }
                }
            }
            else
            {
                var jobPostActivity = _context.JobPostActivities.Where(x => x.SkrId == id);

                if (jobPostActivity == null)
                {
                    return NotFound();
                }
                foreach (JobPostActivity activity in jobPostActivity)
                {
                    var job = _context.JobPosts.FirstOrDefault(x => x.JobPostingId == activity.JobPostingId);
                    if (job != null)
                    {
                        var location = _context.LocationMasters.FirstOrDefault(x => x.LocationId == job.JobLocationId);
                        var type = _context.JobTypes.FirstOrDefault(x => x.JobTypeId == job.JobPostTypeId);
                        var company = _context.EmployerProfiles.FirstOrDefault(x => x.EmpId == job.PostedByEmpId);
                        jobs.Add(new
                        {
                            Title = job.JobTitle,
                            PrimarySkill = job.JobPrimarySkill,
                            SecondarySkill = job.JobSecondarySkill,
                            AppliedDate = activity.ApplicantApplyDate,
                            Location = location?.City,
                            JobType = type?.jobTypeDesc,
                            MinExp = job.MinExp,
                            MaxExp = job.MaxExp,
                            CompanyName = job.IsCompanyNameHidden ? string.Empty : company?.EmpFullName,
                            CompanyLogo = job.IsCompanyNameHidden ? string.Empty : company?.CompanyLogoImage
                        });
                    }
                }
            }
            return Ok(jobs);
        }

        // PUT: api/JobPostActivities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJobPostActivity(int id, JobPostActivity jobPostActivity)
        {
            if (id != jobPostActivity.Id)
            {
                return BadRequest();
            }

            _context.Entry(jobPostActivity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobPostActivityExists(id))
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

        // POST: api/JobPostActivities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public IActionResult PostJobPostActivity(JobPostActivity jobPostActivity)
        {
            jobPostActivity.ApplicantApplyDate = DateTime.Now;
            _context.JobPostActivities.Add(jobPostActivity);
            _context.SaveChanges();

            return Ok(true);
        }

        // DELETE: api/JobPostActivities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobPostActivity(int id)
        {
            var jobPostActivity = await _context.JobPostActivities.FindAsync(id);
            if (jobPostActivity == null)
            {
                return NotFound();
            }

            _context.JobPostActivities.Remove(jobPostActivity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool JobPostActivityExists(int id)
        {
            return _context.JobPostActivities.Any(e => e.Id == id);
        }
    }
}
