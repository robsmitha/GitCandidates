using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Jobs.Queries.GetJobApplication
{
    public class JobModel : IMapFrom<Job>
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string JobTypeName { get; set; }
        public string SeniorityLevelName { get; set; }
        public string Travel { get; set; }
        public bool? AllowRemote { get; set; }
        public string TeamSize { get; set; }
        public int? MinSalary { get; set; }
        public int? MaxSalary { get; set; }
        public string Description { get; set; }
        public int CompanyID { get; set; }
        public string CompanyGitHubLogin { get; set; }
        public string CompanyFounded { get; set; }
        public string CompanySize { get; set; }
        public string CompanyLocation { get; set; }
        public List<JobLocationModel> Locations { get; set; }
    }
}
