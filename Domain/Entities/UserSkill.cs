using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class UserSkill : BaseEntity
    {
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }
        public int SkillID { get; set; }
        [ForeignKey("SkillID")]
        public Skill Skill { get; set; }
    }
}
