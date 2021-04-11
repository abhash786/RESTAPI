using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RManjusha.RestServices.Models;
using RManjusha.RestServices.Helpers;
using RManjusha.RestServices.Models.AuthModels;
using RManjusha.RestServices.Securities;
using RManjusha.RestServices.Interceptors;
using AutoMapper;
using RManjusha.RestServices.Exceptions;
using Microsoft.Extensions.Configuration;

namespace RManjusha.RestServices.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SeekerProfilesController : ControllerBase
    {
        private readonly RManjushaContext _context;
        private readonly SecurityManager _securityManager;
        private readonly JWTHelper _jwtHelper;
        private readonly IMapper _mapper;

        public IConfiguration Configuration { get; }

        public SeekerProfilesController(RManjushaContext context, SecurityManager manager, JWTHelper jwtHelper, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _securityManager = manager;
            _jwtHelper = jwtHelper;
            _mapper = mapper;
            Configuration = configuration;
        }

        // GET: api/SeekerProfiles
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeekerProfile>>> GetSeekerProfiles()
        {
            return await _context.SeekerProfiles.ToListAsync();
        }

        // GET: api/SeekerProfiles/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<SeekerProfile>> GetSeekerProfile(int id)
        {
            var seekerProfile = await _context.SeekerProfiles.FindAsync(id);

            if (seekerProfile == null)
            {
                return NotFound();
            }

            return seekerProfile;
        }

        // PUT: api/SeekerProfiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeekerProfile(int id, SeekerProfile seekerProfile)
        {
            if (id != seekerProfile.SkrId)
            {
                return BadRequest();
            }
            seekerProfile.Password = _context.SeekerProfiles.AsNoTracking().FirstOrDefault(x => x.SkrId == id).Password;
            using (var con = new RManjushaContext(Configuration))
            {
                con.Entry(seekerProfile).State = EntityState.Modified;
                await con.SaveChangesAsync();
            }

            return Ok(seekerProfile);
        }

        // POST: api/SeekerProfiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public IActionResult PostSeekerProfile(SeekerProfile seekerProfile)
        {
            try
            {
                seekerProfile.Dob = new DateTime(1985, 01, 01);
                var login = new LoginModel()
                {
                    Username = seekerProfile.ContactNum != null ? seekerProfile.ContactNum.ToString() :
                    seekerProfile.Aadhaar != null ? seekerProfile.Aadhaar.ToString() : seekerProfile.Email,
                    Password = seekerProfile.Password
                };

                var user = _context.SeekerProfiles.FirstOrDefault(x => (x.ContactNum != null && x.ContactNum == seekerProfile.ContactNum) ||
                (x.Email != null && x.Email == seekerProfile.Email) || (x.Aadhaar != null && x.Aadhaar == seekerProfile.Aadhaar));
                if (user != null)
                    throw new Exception("Unable to create user profile. any one of contact number/Email/Aadhar is duplicate");

                seekerProfile.Password = SimpleEncryptionHelper.Encrypt(seekerProfile.Password);
                _context.SeekerProfiles.Add(seekerProfile);
                _context.SaveChanges();

                var auth = _securityManager.AuthenticateUser(login);
                IActionResult response;
                if (auth != null)
                {
                    auth.Password = string.Empty;
                    response = StatusCode(StatusCodes.Status200OK, new { token = _jwtHelper.GenerateJSONWebToken(auth.SkrCode), User = auth });
                }
                else
                {
                    response = StatusCode(
                           StatusCodes.Status404NotFound,
                           "Unable to create seeker profile");
                }

                return response;
            }
            catch (Exception ex)
            {
                if (ex is DbUpdateException)
                {
                    throw new DataValidationException(ex.GetInnermostException().Message);
                }
                else
                    throw new ServiceException(ex.GetInnermostException().Message);
            }
        }

        // DELETE: api/SeekerProfiles/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeekerProfile(int id)
        {
            var seekerProfile = await _context.SeekerProfiles.FindAsync(id);
            if (seekerProfile == null)
            {
                return NotFound();
            }

            _context.SeekerProfiles.Remove(seekerProfile);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SeekerProfileExists(int id)
        {
            return _context.SeekerProfiles.Any(e => e.SkrId == id);
        }
    }
}
