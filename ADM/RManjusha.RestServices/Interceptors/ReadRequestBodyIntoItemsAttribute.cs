using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IO;
using System.Text;

namespace RManjusha.RestServices.Interceptors
{
    public class ReadRequestBodyIntoItemsAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public new void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context == null) return;

            // NEW! enable sync IO beacuse the JSON reader apparently doesn't use async and it throws an exception otherwise
            var syncIOFeature = context.HttpContext.Features.Get<IHttpBodyControlFeature>();
            if (syncIOFeature != null)
            {
                syncIOFeature.AllowSynchronousIO = true;

                var req = context.HttpContext.Request;

                req.EnableBuffering();

                // read the body here as a workarond for the JSON parser disposing the stream
                if (req.Body.CanSeek)
                {
                    req.Body.Seek(0, SeekOrigin.Begin);

                    // if body (stream) can seek, we can read the body to a string for logging purposes
                    using (var reader = new StreamReader(
                         req.Body,
                         encoding: Encoding.UTF8,
                         detectEncodingFromByteOrderMarks: false,
                         bufferSize: 8192,
                         leaveOpen: true))
                    {
                        var jsonString = reader.ReadToEnd();

                        // store into the HTTP context Items["request_body"]
                        context.HttpContext.Items.Add("request_body", jsonString);
                    }

                    // go back to beginning so json reader get's the whole thing
                    req.Body.Seek(0, SeekOrigin.Begin);
                }
            }
        }
    }
}
