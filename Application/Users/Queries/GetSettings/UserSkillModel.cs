using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Queries.GetSettings
{
    public class UserSkillModel
    {
        public UserSkillModel(SkillModel skill = null, UserSkill userSkill = null)
        {
            Skill = skill ?? new SkillModel();
            UserHasSkill = userSkill?.Active ?? false;
        }
        public SkillModel Skill { get; set; }
        public bool UserHasSkill { get; set; }
    }
}
