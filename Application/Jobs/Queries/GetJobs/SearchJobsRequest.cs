using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Jobs.Queries.GetJobs
{

    public class SearchJobsRequest
    {
        public string keyword { get; set; }
        public double? lat { get; set; }
        public double? lng { get; set; }
        public int? miles { get; set; }
    }
}
