using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Jobs.Queries.SearchJobs
{
    public class SearchJobsModel : IMapFrom<Job>
    {
        public SearchJobsModel()
        {
            Jobs = new List<SearchJobModel>();
        }
        public string DisplayLocation { get; set; }

        public List<SearchJobModel> Jobs { get; set; }
    }
}
