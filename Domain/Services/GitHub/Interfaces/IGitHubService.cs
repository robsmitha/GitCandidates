using Domain.Services.GitHub.Interfaces;
using Domain.Services.GitHub.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.GitHub.Interfaces
{
    public interface IGitHubService
    {
        Task<IAccessToken> GenerateOAuthAccessToken(IGenerateOAuthAccessToken generateOAuthAccessToken);
        Task<IGitHubUser> GetAuthenticatedUser(string accessToken);
        Task<IGitHubUser> GetUser(string username, string accessToken);
        Task<List<Organization>> GetOrganizations(string username, string accessToken);
        Task<IOrganization> GetOrganization(string organization, string accessToken);
    }
}
