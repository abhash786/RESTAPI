using System;
using System.Collections.Generic;

#nullable disable

namespace RManjusha.RestServices.Models
{
    public partial class CourseMaster
    {
        public CourseMaster()
        {
            EducationDetails = new HashSet<EducationDetail>();
        }

        public short CourseId { get; set; }
        public string CourseShortName { get; set; }
        public string CourseFullName { get; set; }

        public virtual ICollection<EducationDetail> EducationDetails { get; set; }
    }
}
