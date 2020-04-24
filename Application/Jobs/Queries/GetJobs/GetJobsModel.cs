using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Jobs.Queries.GetJobs
{
    public class GetJobsModel : IMapFrom<Job>
    {
        public GetJobsModel()
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
        public string Posted
        {
            get
            {
                var dateDiff = DateTime.Now.Subtract(PostAt);
                if ((int)dateDiff.TotalDays > 13) return $"Posted {(int)dateDiff.TotalDays / 7} weeks ago.";
                if ((int)dateDiff.TotalDays == 7) return $"Posted a week ago.";
                if ((int)dateDiff.TotalDays > 1) return $"Posted {(int)dateDiff.TotalDays} days ago.";
                if ((int)dateDiff.TotalDays == 1) return $"Posted a day ago.";
                if ((int)dateDiff.TotalHours > 1) return $"Posted {(int)dateDiff.TotalHours} hours ago.";
                if ((int)dateDiff.TotalHours == 1) return $"Posted one hour ago.";
                return "New";
            }
        }

        /// <summary>
        /// Job has multiple locations. If null, company address displays
        /// </summary>
        public List<JobLocationModel> Locations { get; set; }
    }
}
