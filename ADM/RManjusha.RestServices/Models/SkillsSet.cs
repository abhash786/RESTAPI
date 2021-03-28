using System;
using System.Collections.Generic;

#nullable disable

namespace RManjusha.RestServices.Models
{
    public partial class SkillsSet
    {
        public SkillsSet()
        {
            SeekersSkillsSets = new HashSet<SeekersSkillsSet>();
        }

        public short SkillSetId { get; set; }
        public string SkillSetName { get; set; }
        public string SubSkilllSetName { get; set; }

        public virtual ICollection<SeekersSkillsSet> SeekersSkillsSets { get; set; }
    }
}
