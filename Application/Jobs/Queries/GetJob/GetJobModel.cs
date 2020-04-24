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
        }
        public int ID { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CompanyID { get; set; }
        public string CompanyGitHubLogin { get; set; }

        /// <summary>
        /// Job has multiple locations. If null, company address displays
        /// </summary>
        public List<JobLocationModel> Locations { get;set; }

    }
}
