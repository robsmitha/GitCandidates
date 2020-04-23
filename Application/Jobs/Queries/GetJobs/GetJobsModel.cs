using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Jobs.Queries.GetJobs
{
    public class GetJobsModel : IMapFrom<Job>
    {
        public int ID { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }
        public double Latitide { get; set; }
        public double Longitude { get; set; }
    }
}
