using RManjusha.RestServices.Models;
using RManjusha.RestServices.Models.AuthModels;
using System;
using System.Linq;

namespace RManjusha.RestServices.Helpers
{
    public class SecurityManager
    {
        public SecurityManager(RManjushaContext dbContext)
        {
            DbContext = dbContext;
        }

        public RManjushaContext DbContext { get; }

        public SeekerProfile AuthenticateUser(LoginModel user)
        {
            SeekerProfile authUser = null;
            // Attempt to validate user
            if (Int64.TryParse(user.Username, out long usernameasLong))
            {
                authUser = DbContext.SeekerProfiles.Where(
              u => Convert.ToInt64(u.Aadhaar)
                   == Convert.ToInt64(user.Username)
                   || Convert.ToInt64(u.ContactNum)
                   == Convert.ToInt64(user.Username)
                 ).FirstOrDefault();
            }
            else
                authUser = DbContext.SeekerProfiles.Where(
                   u => u.Email.ToLower() == user.Username.ToLower()).FirstOrDefault();

            if (authUser != null && authUser.Password == SimpleEncryptionHelper.Encrypt(user.Password))
            {
                return authUser;
            }

            return null;
        }

        public EmployerProfile AuthenticateCorporateUser(LoginModel user)
        {
            EmployerProfile authUser = null;
            // Attempt to validate user
            if (Int64.TryParse(user.Username, out long usernameasLong))
            {
                authUser = DbContext.EmployerProfiles.Where(
              u => Convert.ToInt64(u.EmpContactPersonNumber)
                   == Convert.ToInt64(user.Username)
                 ).FirstOrDefault();
            }
            else
                authUser = DbContext.EmployerProfiles.Where(
                   u => u.EmpEmailId.ToLower() == user.Username.ToLower()).FirstOrDefault();

            if (authUser != null && authUser.Password == SimpleEncryptionHelper.Encrypt(user.Password))
            {
                return authUser;
            }

            return null;
        }

        //protected List<AppUserClaim> GetUserClaims(UserAccount authUser)
        //{
        //    List<AppUserClaim> list =
        //      new List<AppUserClaim>();

        //    using (var db = new PtcDbContext())
        //    {
        //        list = db.Claims.Where(
        //               u => u.UserId == authUser.UserId)
        //               .ToList();
        //    }

        //    return list;
        //}

        //protected AppUserAuth BuildUserAuthObject(UserAccount authUser)
        //{
        //    AppUserAuth ret = new AppUserAuth();
        //    List<AppUserClaim> claims =
        //      new List<AppUserClaim>();

        //    // Set User Properties
        //    ret.UserName = authUser.UserName;
        //    ret.IsAuthenticated = true;
        //    ret.BearerToken = new Guid().ToString();

        //    // Get all claims for this user
        //    claims = GetUserClaims(authUser);

        //    // Loop through all claims and
        //    // set properties of user object
        //    foreach (AppUserClaim claim in claims)
        //    {
        //        try
        //        {
        //            typeof(AppUserAuth)
        //              .GetProperty(claim.ClaimType)
        //                .SetValue(ret, Convert.ToBoolean(
        //                  claim.ClaimValue), null);
        //        }
        //        catch
        //        {
        //        }
        //    }

        //    return ret;
        //}
    }
}
