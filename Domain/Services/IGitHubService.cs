using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IGitHubService
    {
        Task<IGitHubAccessToken> GenerateOAuthAccessToken(IGenerateOAuthAccessToken generateOAuthAccessToken);
        Task<IGitHubUser> GetAuthenticatedUser(IGitHubAccessToken accessToken);
    }
}
