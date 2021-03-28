using System;
using System.Collections.Generic;

#nullable disable

namespace RManjusha.RestServices.Models
{
    public partial class EducationDetail
    {
        public int Id { get; set; }
        public int? SkrId { get; set; }
        public string SkrCode { get; set; }
        public short? CourseId { get; set; }
        public string CourseSpecialization { get; set; }
        public string OtherCourseName { get; set; }
        public string InstituteName { get; set; }
        public string UniversityBoardName { get; set; }
        public DateTime CourseStartDate { get; set; }
        public DateTime? CourseCompletionDate { get; set; }
        public double? PercentageOrCgpa { get; set; }

        public virtual CourseMaster Course { get; set; }
        public virtual SeekerProfile Skr { get; set; }
        public virtual SeekerProfile SkrCodeNavigation { get; set; }
    }
}
