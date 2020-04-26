using Application.Common.Mappings;
using Domain.Entities;
using Domain.Services.GitHub.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.GetUser
{
    public class GetUserModel : IMapFrom<User>
    {
        public GetUserModel()
        {
            JobApplications = new List<JobApplicationModel>();
            InactiveJobApplications = new List<JobApplicationModel>();
        }
        public string UserStatusTypeName { get; set; }
        public GitHubUserModel GitHubUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<JobApplicationModel> JobApplications { get; set; }
        public List<JobApplicationModel> InactiveJobApplications { get; set; }
    }
}
