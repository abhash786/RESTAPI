using System;
using System.Collections.Generic;

#nullable disable

namespace RManjusha.RestServices.Models
{
    public partial class LocationMaster
    {
        public LocationMaster()
        {
            JobPosts = new HashSet<JobPost>();
            SeekerProfiles = new HashSet<SeekerProfile>();
        }

        public int LocationId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public virtual ICollection<JobPost> JobPosts { get; set; }
        public virtual ICollection<SeekerProfile> SeekerProfiles { get; set; }
    }
}
