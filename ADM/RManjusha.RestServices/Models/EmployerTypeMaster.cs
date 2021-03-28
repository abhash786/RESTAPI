using System;
using System.Collections.Generic;

#nullable disable

namespace RManjusha.RestServices.Models
{
    public partial class EmployerTypeMaster
    {
        public EmployerTypeMaster()
        {
            EmployerProfiles = new HashSet<EmployerProfile>();
        }

        public byte EmpTypeId { get; set; }
        public string EmpTypeDesc { get; set; }

        public virtual ICollection<EmployerProfile> EmployerProfiles { get; set; }
    }
}
