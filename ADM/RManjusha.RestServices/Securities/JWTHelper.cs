using RManjusha.RestServices.Models.AuthModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using System.Linq;
using RManjusha.RestServices.Models;

namespace RManjusha.RestServices.Securities
{
    public class JWTHelper
    {
        private IConfiguration _config;
        private JwtSettings _settings;
        private readonly RManjushaContext dataContext;

        public JWTHelper(IConfiguration config, JwtSettings settings, RManjushaContext dataContext)
        {
            _config = config;
            _settings = settings;
            this.dataContext = dataContext;
        }

        public string GenerateJSONWebToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _settings.Issuer,
                Audience = _settings.Audience,
                Subject = new ClaimsIdentity(new[] { new Claim("username", username) }),
                Expires = DateTime.Now.AddMinutes(_settings.MinutesToExpiration),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public EmployerProfile ValidateEmployerJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var username = jwtToken.Claims.First(x => x.Type == "username").Value;

                // return account id from JWT token if validation successful
                return dataContext.EmployerProfiles.FirstOrDefault(x => x.EmpCode == username);
            }
            catch (Exception ex)
            {
                // return null if validation fails
                return null;
            }
        }

        public bool ValidaterJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var username = jwtToken.Claims.First(x => x.Type == "username").Value;

                // return account id from JWT token if validation successful
                return true;
            }
            catch (Exception ex)
            {
                // return null if validation fails
                return false;
            }
        }

        public SeekerProfile ValidateSeekerJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var username = jwtToken.Claims.First(x => x.Type == "username").Value;

                // return account id from JWT token if validation successful
                return dataContext.SeekerProfiles.FirstOrDefault(x => x.SkrCode == username);
            }
            catch (Exception ex)
            {
                // return null if validation fails
                return null;
            }
        }
    }
}
