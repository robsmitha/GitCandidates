using Domain.Services.GitHub.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Queries.GetOrganization
{
    public class GetOrganizationResponse
    {
        public GetOrganizationResponse(GitHubOrganizationModel organization)
        {
            GitHubOrganization = organization ?? new GitHubOrganizationModel();
        }
        public GitHubOrganizationModel GitHubOrganization { get; set; }
    }
}
