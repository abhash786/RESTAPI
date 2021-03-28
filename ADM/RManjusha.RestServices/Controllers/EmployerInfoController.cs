using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RManjusha.RestServices.Helpers;
using RManjusha.RestServices.Interceptors;
using RManjusha.RestServices.Models;
using RManjusha.RestServices.Models.AuthModels;
using RManjusha.RestServices.Securities;

namespace RManjusha.RestServices.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EmployerInfoController : ControllerBase
    {
        private readonly RManjushaContext _context;
        private SecurityManager _securityManager;
        private JWTHelper _jwtHelper;

        public EmployerInfoController(RManjushaContext context, SecurityManager securityManager, JWTHelper jwtHelper)
        {
            _context = context;
            _securityManager = securityManager;
            _jwtHelper = jwtHelper;
        }

        // GET: api/EmployerInfo/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployerProfile>> GetEmployerInfo(int id)
        {
            var employerInfo = await _context.EmployerProfiles.FindAsync(id);

            if (employerInfo == null)
            {
                return NotFound();
            }

            return employerInfo;
        }

        // PUT: api/EmployerInfo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployerInfo(int id, EmployerProfile employerInfo)
        {
            if (id != employerInfo.EmpId)
            {
                return BadRequest();
            }
            var emp = _context.EmployerProfiles.FirstOrDefault(x => x.EmpId == id);
            if (emp != null)
            {
                employerInfo.Password = emp.Password;
                employerInfo.CompanyLogoImage = emp.CompanyLogoImage;
            }
            using (var con = new RManjushaContext())
            {
                con.Entry(employerInfo).State = EntityState.Modified;
                await con.SaveChangesAsync();
            }

            return Ok(employerInfo);
        }

        // POST: api/EmployerInfo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public IActionResult PostEmployerInfo(EmployerProfile employerInfo)
        {
            var login = new LoginModel()
            {
                Username = employerInfo.EmpEmailId,
                Password = employerInfo.Password,
                IsCorporate = true
            };
            var emp = _context.EmployerProfiles.FirstOrDefault(x=>x.EmpEmailId == employerInfo.EmpEmailId || x.EmpPan == employerInfo.EmpPan
            || x.EmpGstin == employerInfo.EmpGstin);

            if (emp != null)
                throw new Exception("Unable to create employer profile. Email/PAN/GST are duplicate");

            employerInfo.Password = SimpleEncryptionHelper.Encrypt(employerInfo.Password);
            _context.EmployerProfiles.Add(employerInfo);
            _context.SaveChanges();

            var auth = _securityManager.AuthenticateCorporateUser(login);
            IActionResult response;
            if (auth != null)
            {
                auth.Password = string.Empty;
                response = StatusCode(StatusCodes.Status200OK, new { token = _jwtHelper.GenerateJSONWebToken(auth.EmpCode), User = auth });
            }
            else
            {
                response = StatusCode(
                       StatusCodes.Status404NotFound,
                       "Unable to create employer profile.");
            }

            return response;
        }

        // DELETE: api/EmployerInfo/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployerInfo(int id)
        {
            var employerInfo = await _context.EmployerProfiles.FindAsync(id);
            if (employerInfo == null)
            {
                return NotFound();
            }

            _context.EmployerProfiles.Remove(employerInfo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployerInfoExists(int id)
        {
            return _context.EmployerProfiles.Any(e => e.EmpId == id);
        }
    }
}
