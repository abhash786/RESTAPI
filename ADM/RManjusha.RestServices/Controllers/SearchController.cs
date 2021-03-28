using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RManjusha.RestServices.Interceptors;
using RManjusha.RestServices.Models;
using RManjusha.RestServices.Securities;
using System;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RManjusha.RestServices.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly RManjushaContext _context;
        public SearchController(RManjushaContext context)
        {
            _context = context;
        }

        // GET api/<SearchController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var employerInfo = _context.EmployerProfiles.Find(id);

            if (employerInfo == null)
            {
                return NotFound();
            }

            return Ok(employerInfo);
        }

        // POST api/<SearchController>
        [HttpPost]
        public IActionResult Post([FromBody] SearchInput input)
        {
            IActionResult response = null;

            if (input.IsJob)
            {
                var jobs = _context.JobPosts.AsQueryable();
                if (!string.IsNullOrEmpty(input.Location))
                {
                    jobs = jobs.Where(x =>
                    x.JobLocation.City == input.Location ||
                     x.JobLocation.State == input.Location ||
                     x.JobLocation.Country == input.Location);
                }
                if (!string.IsNullOrEmpty(input.Keyword))
                {
                    jobs = jobs.Where(x =>
                    x.JobPrimarySkill.Contains(input.Keyword) ||
                    x.JobSecondarySkill.Contains(input.Keyword));
                }
                else
                    jobs = new List<JobPost>().AsQueryable();
                var output = jobs.ToList();

                response = StatusCode(StatusCodes.Status200OK, new { jobs = output });
            }
            else
            {
                var candidates = _context.SeekerProfiles.AsQueryable();
                if (!string.IsNullOrEmpty(input.Location))
                {
                    candidates = candidates.Where(x => x.JobLocationPrefNavigation.City == input.Location ||
                     x.JobLocationPrefNavigation.State == input.Location ||
                     x.JobLocationPrefNavigation.Country == input.Location);
                }
                if (!string.IsNullOrEmpty(input.Keyword))
                {
                    candidates = candidates.Where(x =>
                    x.SeekersSkillsSetSkrCodeNavigations.Any(s => s.SkillSet.SkillSetName.Contains(input.Keyword) ||
                    s.SkillSet.SubSkilllSetName.Contains(input.Keyword)));
                }
                else
                    candidates = new List<SeekerProfile>().AsQueryable();
                var output = candidates.ToList();
                if (output.Count() > 0)
                {
                    foreach (var user in output)
                    {
                        user.Password = "";
                        user.ExperienceDetailSkrs = _context.ExperienceDetails.Where(x => x.SkrCode == user.SkrCode).ToList();
                        user.EducationDetailSkrs = _context.EducationDetails.Where(x => x.SkrCode == user.SkrCode).ToList();
                        user.SeekersSkillsSetSkrs = _context.SeekersSkillsSets.Where(x => x.SkrCode == user.SkrCode).ToList();
                        //if (user.SeekerImage != null && !user.SeekerImage.Contains("https://"))
                        //    user.SeekerImage = $"https://{Request.Host.Value}/userfiles/{user.SeekerImage}";
                        //if (user.ResumeCv != null && !user.ResumeCv.Contains("https://"))
                        //    user.ResumeCv = $"https://{Request.Host.Value}/userfiles/{user.ResumeCv}";
                    }
                }
                response = StatusCode(StatusCodes.Status200OK, new { candidates = output });
            }

            return response;
        }

        // PUT api/<SearchController>/5
        [HttpPut]
        public IActionResult Put([FromBody] int id)
        {
            var employerInfo = _context.EmployerProfiles.Find(id);

            if (employerInfo == null)
            {
                return NotFound();
            }
            return Ok(new
            {
                name = employerInfo.EmpFullName,
                logo = employerInfo.CompanyLogoImage
            });
        }

        // DELETE api/<SearchController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
