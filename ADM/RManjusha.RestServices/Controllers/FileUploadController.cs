using RManjusha.RestServices.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using RManjusha.RestServices.Securities;
using RManjusha.RestServices.Interceptors;

namespace RManjusha.RestServices.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly SeekerProfilesController _seekerProfiles;
        private IWebHostEnvironment _hostingEnvironment;
        private readonly EmployerInfoController employerInfoController;

        public FileUploadController(SeekerProfilesController seekerProfiles, IWebHostEnvironment hostingEnvironment, EmployerInfoController employerInfoController)
        {
            _seekerProfiles = seekerProfiles;
            _hostingEnvironment = hostingEnvironment;
            this.employerInfoController = employerInfoController;
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFile()
        {
            try
            {
                var file = Request.Form.Files[0];
                string folderName = "Upload";
                string webRootPath = _hostingEnvironment.ContentRootPath;
                string newPath = Path.Combine(webRootPath, folderName);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(newPath, fileName);

                    if (System.IO.File.Exists(fullPath))
                        System.IO.File.Delete(fullPath);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    var filenameParts = fileName.Split("_");
                    if (int.TryParse(filenameParts[0], out int id))
                    {
                        if (filenameParts[1].Contains("Resume", System.StringComparison.InvariantCultureIgnoreCase))
                        {
                            var user = _seekerProfiles.GetSeekerProfile(id)?.Result?.Value;
                            user.ResumeCv = $"https://{Request.Host.Value}/userfiles/{fileName}";
                            await _seekerProfiles.PutSeekerProfile(id, user);
                        }
                        else if (filenameParts[1].Contains("Photo", System.StringComparison.InvariantCultureIgnoreCase))
                        {
                            var user = _seekerProfiles.GetSeekerProfile(id)?.Result?.Value;
                            user.SeekerImage = $"https://{Request.Host.Value}/userfiles/{fileName}";
                            await _seekerProfiles.PutSeekerProfile(id, user);
                        }
                        else if (filenameParts[1].Contains("logo", System.StringComparison.InvariantCultureIgnoreCase))
                        {
                            var emp = employerInfoController.GetEmployerInfo(id)?.Result?.Value;
                            if (emp != null)
                            {
                                emp.CompanyLogoImage = $"https://{Request.Host.Value}/userfiles/{fileName}";
                                await employerInfoController.PutEmployerInfo(id, emp);
                            }
                        }
                    }
                    else if (filenameParts[1].Contains("logo", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var user = employerInfoController.GetEmployerInfo(id)?.Result?.Value;
                        if (user != null)
                        {
                            user.CompanyLogoImage = fileName;
                            await employerInfoController.PutEmployerInfo(id, user);
                        }
                    }
                }
                return Ok(StatusCode(StatusCodes.Status200OK, "Upload Successful."));
            }
            catch (Exception ex)
            {
                return BadRequest(StatusCode(
                       StatusCodes.Status500InternalServerError,
                       "Upload Failed: " + ex.GetInnermostException().Message));
            }
        }
    }
}
