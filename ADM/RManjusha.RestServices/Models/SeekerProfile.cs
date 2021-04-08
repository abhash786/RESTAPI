using System;
using System.Collections.Generic;

#nullable disable

namespace RManjusha.RestServices.Models
{
    public partial class SeekerProfile
    {
        public SeekerProfile()
        {
            EducationDetailSkrCodeNavigations = new HashSet<EducationDetail>();
            EducationDetailSkrs = new HashSet<EducationDetail>();
            ExperienceDetailSkrCodeNavigations = new HashSet<ExperienceDetail>();
            ExperienceDetailSkrs = new HashSet<ExperienceDetail>();
            JobPostActivities = new HashSet<JobPostActivity>();
            SeekersSkillsSetSkrCodeNavigations = new HashSet<SeekersSkillsSet>();
            SeekersSkillsSetSkrs = new HashSet<SeekersSkillsSet>();
        }

        public int SkrId { get; set; }
        public string SkrCode { get; set; }
        public byte? SkrTypeId { get; set; }
        public DateTime SkrProfileCreateDate { get; set; }
        public DateTime? IValueRegDate { get; set; }
        public DateTime? ICatalystRegDate { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? Dob { get; set; }
        public string Gender { get; set; }
        public bool SkrProfileVisibility { get; set; }
        public decimal? ContactNum { get; set; }
        public decimal? AltContactNum { get; set; }
        public string SeekerImage { get; set; }
        public decimal? Aadhaar { get; set; }
        public string SpokenLanguage { get; set; }
        public string CommAdd { get; set; }
        public string PermAdd { get; set; }
        public int? JobLocationPref { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int? Income { get; set; }
        public bool? IsYearly { get; set; }
        public string Currency { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string ResumeCv { get; set; }
        public string FbId { get; set; }
        public string InstaId { get; set; }
        public string TwtrId { get; set; }
        public string LkdnId { get; set; }
        public string CvHeadLine { get; set; }

        public virtual LocationMaster JobLocationPrefNavigation { get; set; }
        public virtual SeekerType SkrType { get; set; }
        public virtual ICollection<EducationDetail> EducationDetailSkrCodeNavigations { get; set; }
        public virtual ICollection<EducationDetail> EducationDetailSkrs { get; set; }
        public virtual ICollection<ExperienceDetail> ExperienceDetailSkrCodeNavigations { get; set; }
        public virtual ICollection<ExperienceDetail> ExperienceDetailSkrs { get; set; }
        public virtual ICollection<JobPostActivity> JobPostActivities { get; set; }
        public virtual ICollection<SeekersSkillsSet> SeekersSkillsSetSkrCodeNavigations { get; set; }
        public virtual ICollection<SeekersSkillsSet> SeekersSkillsSetSkrs { get; set; }
    }
}
