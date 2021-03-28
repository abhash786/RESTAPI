using RManjusha.RestServices.Helpers;
using RManjusha.RestServices.Models.AuthModels;
using RManjusha.RestServices.Securities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RManjusha.RestServices.Interceptors;
using RManjusha.RestServices.Exceptions;

namespace RManjusha.RestServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private IConfiguration _config;

        private JwtSettings _settings;
        private readonly SecurityManager _manager;
        private readonly JWTHelper _jwtHelper;

        public SecurityController(IConfiguration config, JwtSettings settings, SecurityManager manager, JWTHelper jwtHelper)
        {
            _config = config;
            _settings = settings;
            _manager = manager;
            _jwtHelper = jwtHelper;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginModel login)
        {
            IActionResult response;
            if (login.IsCorporate)
            {
                var auth = _manager.AuthenticateCorporateUser(login);

                if (auth != null)
                {
                    auth.Password = string.Empty;
                    //if (auth.CompanyLogoImage != null && !auth.CompanyLogoImage.Contains("https://", System.StringComparison.InvariantCultureIgnoreCase))
                    //    auth.CompanyLogoImage = $"https://{Request.Host.Value}/userfiles/{auth.CompanyLogoImage}";

                    response = StatusCode(StatusCodes.Status200OK, new { token = _jwtHelper.GenerateJSONWebToken(auth.EmpCode), User = auth });
                }
                else
                {
                    throw new ServiceException(
                           "Invalid User Name/Password.");
                }
            }
            else
            {
                var auth = _manager.AuthenticateUser(login);

                if (auth != null)
                {
                    auth.Password = string.Empty;
                    //if (auth.SeekerImage !=null && !auth.SeekerImage.Contains("https://", System.StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    auth.SeekerImage = $"https://{Request.Host.Value}/userfiles/{auth.SeekerImage}";
                    //}
                    //if (auth.ResumeCv != null && !auth.ResumeCv.Contains("https://", System.StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    auth.ResumeCv = $"https://{Request.Host.Value}/userfiles/{auth.ResumeCv}";
                    //}
                    response = StatusCode(StatusCodes.Status200OK, new { token = _jwtHelper.GenerateJSONWebToken(auth.SkrCode), User = auth });
                }
                else
                {
                    throw new ServiceException(
                           "Invalid User Name/Password.");
                }
            }


            return response;
        }

    }
}

