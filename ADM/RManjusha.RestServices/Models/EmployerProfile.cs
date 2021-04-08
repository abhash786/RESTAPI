using System;
using System.Collections.Generic;

#nullable disable

namespace RManjusha.RestServices.Models
{
    public partial class EmployerProfile
    {
        public EmployerProfile()
        {
            JobPosts = new HashSet<JobPost>();
        }

        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public DateTime EmpProfileCreationDate { get; set; }
        public byte EmpTypeId { get; set; }
        public DateTime? WeValueRegDate { get; set; }
        public DateTime? WeGrowRegDate { get; set; }
        public byte IncId { get; set; }
        public string EmpFullName { get; set; }
        public string EmpEmailId { get; set; }
        public string Password { get; set; }
        public decimal EmpContactNo { get; set; }
        public string EmpAddress { get; set; }
        public DateTime EmpIncorporationDate { get; set; }
        public short BusinessStreamId { get; set; }
        public string EmpGstin { get; set; }
        public string EmpPan { get; set; }
        public string EmpDepartment { get; set; }
        public string EmpContactPersonName { get; set; }
        public decimal EmpContactPersonNumber { get; set; }
        public decimal? EmpContactPersonAltNumber { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string CompanyLogoImage { get; set; }
        public string FbId { get; set; }
        public string InstaId { get; set; }
        public string TwtrId { get; set; }
        public string LkdnId { get; set; }
        public string EmpWebsite { get; set; }
        public string EmpAboutUs { get; set; }
        public string EmploymentNumber { get; set; }

        public virtual BusinessStream BusinessStream { get; set; }
        public virtual EmployerTypeMaster EmpType { get; set; }
        public virtual IncorporationType Inc { get; set; }
        public virtual ICollection<JobPost> JobPosts { get; set; }
    }
}
