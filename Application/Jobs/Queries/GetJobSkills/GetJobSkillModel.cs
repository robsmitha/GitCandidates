using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Jobs.Queries.GetJobSkills
{
    public class GetJobSkillModel : IMapFrom<JobSkill>
    {
        public string SkillName { get; set; }
        public string SkillDescription { get; set; }
        public bool UserHasSkill { get; set; }
    }
}
