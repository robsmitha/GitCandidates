using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Queries.GetJobApplications
{
    public class GetJobApplicationsModel
    {
        public GetJobApplicationsModel()
        {
            JobApplications = new List<JobApplicationModel>();
            InactiveJobApplications = new List<JobApplicationModel>();
        }
        public List<JobApplicationModel> JobApplications { get; set; }
        public List<JobApplicationModel> InactiveJobApplications { get; set; }
    }
}
