using System;
using System.Collections.Generic;

#nullable disable

namespace RManjusha.RestServices.Models
{
    public partial class SeekerType
    {
        public SeekerType()
        {
            SeekerProfiles = new HashSet<SeekerProfile>();
        }

        public byte SkrTypeId { get; set; }
        public string SkrTypeDesc { get; set; }

        public virtual ICollection<SeekerProfile> SeekerProfiles { get; set; }
    }
}
