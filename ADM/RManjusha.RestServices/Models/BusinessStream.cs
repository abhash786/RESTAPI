using System;
using System.Collections.Generic;

#nullable disable

namespace RManjusha.RestServices.Models
{
    public partial class BusinessStream
    {
        public BusinessStream()
        {
            EmployerProfiles = new HashSet<EmployerProfile>();
        }

        public short BusinessStreamId { get; set; }
        public string BusinessStreamName { get; set; }

        public virtual ICollection<EmployerProfile> EmployerProfiles { get; set; }
    }
}
