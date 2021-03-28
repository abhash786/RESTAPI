using System;
using System.Collections.Generic;

#nullable disable

namespace RManjusha.RestServices.Models
{
    public partial class JobType
    {
        public JobType()
        {
            JobPosts = new HashSet<JobPost>();
        }

        public short Id { get; set; }
        public short JobTypeId { get; set; }
        public string jobTypeDesc { get; set; }

        public virtual ICollection<JobPost> JobPosts { get; set; }
    }
}
