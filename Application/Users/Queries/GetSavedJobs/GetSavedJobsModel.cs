using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Queries.GetSavedJobs
{
    public class GetSavedJobsModel : IMapFrom<SavedJob>
    {
        public string JobName { get; set; }
        public string JobDescription { get; set; }
        public string JobCompanyGitHubLogin { get; set; }
        public int JobCompanyID { get; set; }
        public int JobID { get; set; }
        public DateTime JobPostAt { get; set; }
        public string Posted => JobPostAt.ToElapsedTime();
    }
}
