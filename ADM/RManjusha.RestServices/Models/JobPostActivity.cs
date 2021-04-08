using System;
using System.Collections.Generic;

#nullable disable

namespace RManjusha.RestServices.Models
{
    public partial class JobPostActivity
    {
        public int Id { get; set; }
        public int? SkrId { get; set; }
        public int JobPostingId { get; set; }
        public DateTime ApplicantApplyDate { get; set; }

        public virtual JobPost JobPosting { get; set; }
        public virtual SeekerProfile Skr { get; set; }
    }
}
