using Application.Common.Mappings;
using Domain.Services.GitHub.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Queries.GetOrganization
{
    public class GitHubOrganizationModel : IMapFrom<IOrganization>
    {
        public int id { get; set; }
        public string url { get; set; }
        public string login { get; set; }
        public string description { get; set; }
        public string avatar_url { get; set; }
    }
}
