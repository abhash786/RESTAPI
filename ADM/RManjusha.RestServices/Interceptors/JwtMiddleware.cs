using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using RManjusha.RestServices.Models;
using RManjusha.RestServices.Models.AuthModels;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RManjusha.RestServices.Interceptors
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, JwtSettings appSettings)
        {
            _next = next;
            _appSettings = appSettings;
        }

        public async Task Invoke(HttpContext context, RManjushaContext dataContext)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await attachAccountToContext(context, dataContext, token);

            await _next(context);
        }

        private async Task attachAccountToContext(HttpContext context, RManjushaContext dataContext, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Key)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var accountId = jwtToken.Claims.FirstOrDefault(x => x.Type == "username").Value;

                // attach account to context on successful jwt validation
                object account = dataContext.SeekerProfiles.FirstOrDefault(x=>x.SkrCode == accountId);
                if (account == null)
                {
                    account = dataContext.EmployerProfiles.FirstOrDefault(x => x.EmpCode == accountId);
                }
                context.Items["Account"] = account;
            }
            catch(Exception ex)
            {
                throw new Exception("Token expired.", ex);
                // do nothing if jwt validation fails
                // account is not attached to context so request won't have access to secure routes
            }
        }
    }
}
