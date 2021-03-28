using System;
using System.Collections.Generic;

#nullable disable

namespace RManjusha.RestServices.Models
{
    public partial class ExperienceDetail
    {
        public int Id { get; set; }
        public int SkrId { get; set; }
        public string SkrCode { get; set; }
        public byte SkrTypeId { get; set; }
        public DateTime JoiningDate { get; set; }
        public bool IsCurrentJob { get; set; }
        public DateTime? LeavingDate { get; set; }
        public string JobTitle { get; set; }
        public string EmpName { get; set; }
        public string JobCity { get; set; }
        public string JobState { get; set; }
        public string JobCountry { get; set; }
        public string JobProjectDesc { get; set; }
        public string ExpType { get; set; }

        public virtual SeekerProfile Skr { get; set; }
        public virtual SeekerProfile SkrCodeNavigation { get; set; }
    }
}
