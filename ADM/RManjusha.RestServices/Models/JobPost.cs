using System;
using System.Collections.Generic;

#nullable disable

namespace RManjusha.RestServices.Models
{
    public partial class JobPost
    {
        public JobPost()
        {
            JobPostActivities = new HashSet<JobPostActivity>();
        }

        public int Id { get; set; }
        public int JobPostingId { get; set; }
        public string JobPostingCode { get; set; }
        public int? PostedByEmpId { get; set; }
        public short? JobPostTypeId { get; set; }
        public bool IsCompanyNameHidden { get; set; }
        public DateTime JobCreatedDate { get; set; }
        public string JobDescription { get; set; }
        public int JobLocationId { get; set; }
        public bool IsJobActive { get; set; }
        public string JobPrimarySkill { get; set; }
        public string JobSecondarySkill { get; set; }
        public byte? MinExp { get; set; }
        public byte? MaxExp { get; set; }
        public string JobTitle { get; set; }
        public double? PkgRangeFrom { get; set; }
        public double? PkgRangeTo { get; set; }
        public string DesiredEdu { get; set; }

        public virtual LocationMaster JobLocation { get; set; }
        public virtual JobType JobPostType { get; set; }
        public virtual EmployerProfile PostedByEmp { get; set; }
        public virtual ICollection<JobPostActivity> JobPostActivities { get; set; }
    }
}
