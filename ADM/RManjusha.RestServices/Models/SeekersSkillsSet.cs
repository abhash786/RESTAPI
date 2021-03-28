using System;
using System.Collections.Generic;

#nullable disable

namespace RManjusha.RestServices.Models
{
    public partial class SeekersSkillsSet
    {
        public short Id { get; set; }
        public int SkrId { get; set; }
        public string SkrCode { get; set; }
        public short SkillSetId { get; set; }
        public int? SkillLevel { get; set; }

        public virtual SkillsSet SkillSet { get; set; }
        public virtual SeekerProfile Skr { get; set; }
        public virtual SeekerProfile SkrCodeNavigation { get; set; }
    }
}
