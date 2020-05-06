using Domain.Services.GitHub.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Queries.GetOrganizations
{
    public class GetOrganizationsResponse
    {
        public GetOrganizationsResponse(List<Organization> organizations)
        {
            Organizations = organizations ?? new List<Organization>();
        }
        public List<Organization> Organizations { get; set; }
    }
}
