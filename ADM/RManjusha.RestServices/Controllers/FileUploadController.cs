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
        private readonly BlobStorageService storageService;

        public FileUploadController(SeekerProfilesController seekerProfiles, IWebHostEnvironment hostingEnvironment, EmployerInfoController employerInfoController, BlobStorageService storageService)
        {
            _seekerProfiles = seekerProfiles;
            _hostingEnvironment = hostingEnvironment;
            this.employerInfoController = employerInfoController;
            this.storageService = storageService;
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
                    //byte[] bytes = System.IO.File.ReadAllBytes(fullPath);
                    //var storagepath = storageService.UploadFileToBlob(fullPath, bytes, null);
                    var fileurl = HttpContext.Request.Scheme +"://"+ HttpContext.Request.Host.Value + "/Assets/" + fileName;
                    var filenameParts = fileName.Split("_");

                    if (int.TryParse(filenameParts[0], out int id))
                    {
                        if (filenameParts[1].Contains("Resume", StringComparison.InvariantCultureIgnoreCase))
                        {
                            var user = _seekerProfiles.GetSeekerProfile(id)?.Result?.Value;
                            user.ResumeCv = fileurl;
                            await _seekerProfiles.PutSeekerProfile(id, user);
                        }
                        else if (filenameParts[1].Contains("Photo", StringComparison.InvariantCultureIgnoreCase))
                        {
                            var user = _seekerProfiles.GetSeekerProfile(id)?.Result?.Value;
                            user.SeekerImage = fileurl;
                            await _seekerProfiles.PutSeekerProfile(id, user);
                        }
                        else if (filenameParts[1].Contains("logo", StringComparison.InvariantCultureIgnoreCase))
                        {
                            var emp = employerInfoController.GetEmployerInfo(id)?.Result?.Value;
                            if (emp != null)
                            {
                                emp.CompanyLogoImage = fileurl;
                                await employerInfoController.PutEmployerInfo(id, emp);
                            }
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
