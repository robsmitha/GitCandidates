using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Queries.GetSettings
{
    public class GetSettingsModel
    {
        public GetSettingsModel()
        {
            Skills = new List<UserSkillModel>();
        }
        public List<UserSkillModel> Skills { get; set; }
    }
}
