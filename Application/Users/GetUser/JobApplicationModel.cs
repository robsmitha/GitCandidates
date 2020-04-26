using Application.Common.Mappings;
using Domain.Entities;
using System;

namespace Application.Users.GetUser
{
    public class JobApplicationModel : IMapFrom<JobApplication>
    {
        public int ID { get; set; }
        public string JobApplicationStatusTypeName { get; set; }
        public int JobID { get; set; }
        public string JobName { get; set; }
        public string JobCompanyGitHubLogin { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public string UpdatedOn => ModifiedTime != null
            ? ModifiedTime.Value.ToShortDateString()
            : CreatedAt.ToShortDateString();
    }
}