using Domain.Services.GitHub.Interfaces;
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
    }
}
