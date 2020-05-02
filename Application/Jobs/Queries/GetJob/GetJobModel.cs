using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Jobs.Queries.GetJob
{
    public class GetJobModel : IMapFrom<Job>
    {
        public GetJobModel()
        {
            Locations = new List<JobLocationModel>();
            Benefits = new List<BenefitModel>();
            Responsibilities = new List<ResponsibilityModel>();
            Requirements = new List<RequirementModel>();
            Methods = new List<MethodModel>();
        }
        public int ID { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PostHTML { get; set; }
        public int CompanyID { get; set; }
        public string CompanyGitHubLogin { get; set; }
        public DateTime PostAt { get; set; }
        public bool? AllowRemote { get; set; }
        public string TeamSize { get; set; }
        public int? MinSalary { get; set; }
        public int? MaxSalary { get; set; }
        public string Travel { get; set; }
        public string SeniorityLevelName { get; set; }
        public string JobTypeName { get; set; }
        public string Posted => PostAt.ToElapsedTime();
        public bool UserHasActiveApplication { get; set; }
        public int SavedJobID { get; set; }
        public bool IsAcceptingApplications { get; set; }
        public List<JobLocationModel> Locations { get;set; }
        public List<BenefitModel> Benefits { get; set; }
        public List<ResponsibilityModel> Responsibilities { get; set; }
        public List<RequirementModel> Requirements { get; set; }
        public List<MethodModel> Methods { get; set; }
    }
}
