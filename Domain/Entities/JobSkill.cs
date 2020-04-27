using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class JobSkill : BaseEntity
    {
        public int JobID { get; set; }
        [ForeignKey("JobID")]
        public Job Job { get; set; }
        public int SkillID { get; set; }
        [ForeignKey("SkillID")]
        public Skill Skill { get; set; }
    }
}
