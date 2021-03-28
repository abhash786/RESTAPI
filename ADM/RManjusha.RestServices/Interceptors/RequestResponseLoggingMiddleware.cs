using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RManjusha.RestServices.Exceptions;
using RManjusha.RestServices.Helpers;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RManjusha.RestServices.Interceptors
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public RequestResponseLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory
                      .CreateLogger<RequestResponseLoggingMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation($"Request received: {DateTime.Now.ToString()} ");
            //First, get the incoming request
            var request = await FormatRequest(context.Request);
            _logger.LogInformation(request);
            //Copy a pointer to the original response body stream
            var originalBodyStream = context.Response.Body;

            //Create a new memory stream...
            using (var responseBody = new MemoryStream())
            {
                //...and use that for the temporary response body
                context.Response.Body = responseBody;
                try
                {
                    //Continue down the Middleware pipeline, eventually returning to this class
                    await _next(context);
                }
                catch (Exception ex)
                {
                    await HandleExceptionMessageAsync(context, ex).ConfigureAwait(false);
                }
                //Format the response from the server
                var response = await FormatResponse(context.Response);

                //Save log to chosen datastore
                _logger.LogInformation(response);

                //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
                await responseBody.CopyToAsync(originalBodyStream);
                _logger.LogInformation($"Request completed: {DateTime.Now.ToString()} ");
            }
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            ////This line allows us to set the reader for the request back at the beginning of its stream.
            //var body = request.Body;

            ////We now need to read the request stream.  First, we create a new byte[] with the same length as the request stream...

            //var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            ////...Then we copy the entire request stream into the new buffer.
            //await request.Body.ReadAsync(buffer, 0, buffer.Length);

            ////We convert the byte[] into a string using UTF8 encoding...
            //var bodyAsText = Encoding.UTF8.GetString(buffer);

            ////..and finally, assign the read body back to the request body, which is allowed because of EnableRewind()
            //request.Body = body;
            return $"Http Request Information:{Environment.NewLine}" +
                                   $"Schema:{request.Scheme} {Environment.NewLine}" +
                                   $"Host: {request.Host} {Environment.NewLine}" +
                                   $"Path: {request.Path} {Environment.NewLine}" +
                                   $"QueryString: {request.QueryString} {Environment.NewLine}";
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            //We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);

            //...and copy it into a string
            string text = await new StreamReader(response.Body).ReadToEndAsync();

            //We need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);

            //Return the string for the response, including the status code (e.g. 200, 404, 401, etc.)
            return $"Http Response Information:{Environment.NewLine}{response.StatusCode}: {text}";
        }

        private static Task HandleExceptionMessageAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var statusCode = HttpStatusCode.InternalServerError;
            String message = String.Empty;

            var exceptionType = exception.GetType();
            if (exceptionType == typeof(UnauthorizedAccessException))
            {
                message = $"Unauthorized Access: {exception.Message}";
                statusCode = HttpStatusCode.Unauthorized;
            }
            else if (exceptionType == typeof(NotImplementedException))
            {
                message = "A server error occurred.";
                statusCode = HttpStatusCode.NotImplemented;
            }
            else if (exceptionType == typeof(ServiceException))
            {
                message = exception.ToString();
                statusCode = HttpStatusCode.InternalServerError;
            }
            else if (exceptionType == typeof(DataValidationException))
            {
                message = exception.Message;
                statusCode = HttpStatusCode.BadRequest;
            }
            else if (exceptionType == typeof(InvalidOperationException))
            {
                message = "There is an error at service side. Please try after some time.";
                statusCode = HttpStatusCode.BadRequest;
            }
            else
            {
                message = exception.Message;
                statusCode = HttpStatusCode.NotFound;
            }

            var result = JsonConvert.SerializeObject(new
            {
                StatusCode = statusCode,
                Message = message,
                Exception = exception.GetInnermostException(),
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(result);
        }
    }
}
