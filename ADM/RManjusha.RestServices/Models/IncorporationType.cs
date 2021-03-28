using System;
using System.Collections.Generic;

#nullable disable

namespace RManjusha.RestServices.Models
{
    public partial class IncorporationType
    {
        public IncorporationType()
        {
            EmployerProfiles = new HashSet<EmployerProfile>();
        }

        public byte IncId { get; set; }
        public string IncDesc { get; set; }

        public virtual ICollection<EmployerProfile> EmployerProfiles { get; set; }
    }
}
