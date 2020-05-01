using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Jobs.Queries.SearchJobs
{
    public class SearchJobModel : IMapFrom<Job>
    {
        public SearchJobModel()
        {
            Locations = new List<JobLocationModel>();
        }

        public int ID { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CompanyID { get; set; }
        public string CompanyGitHubLogin { get; set; }
        public DateTime PostAt { get; set; }
        public string Posted => PostAt.ToElapsedTime();
        public List<JobLocationModel> Locations { get; set; }
    }
}
