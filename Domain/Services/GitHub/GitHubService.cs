using Domain.Services.Configuration.Interfaces;
using Domain.Services.Configuration.Models;
using Domain.Services.GitHub.Interfaces;
using Domain.Services.GitHub.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Domain.Services.GitHub
{
    public class GitHubService : IGitHubService
    {
        public const string OAuthBaseUrl = "https://github.com/login/oauth/authorize?";
        public const string OAuthGenerateAccessTokenBaseUrl = "https://github.com/login/oauth/access_token";

        private readonly IAppSettings _appSettings;
        private readonly IGitHubClient _client;
        public GitHubService(IOptions<AppSettings> appSettings, IGitHubClient client)
        {
            _appSettings = appSettings.Value;
            _client = client;
        }
        public async Task<IGitHubUser> GetAuthenticatedUser(string accessToken)
        {
            return await _client.GetAsync<GitHubUser>(accessToken, "/user");
        }

        public async Task<IGitHubUser> GetUser(string username, string accessToken)
        {
            return await _client.GetAsync<GitHubUser>(accessToken, $"/users/{username}");
        }

        public async Task<IAccessToken> GenerateOAuthAccessToken(IGenerateOAuthAccessToken generateOAuthAccessToken)
        {
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            var request = new
            {
                generateOAuthAccessToken.code,
                client_id = _appSettings.GitHubClientID,
                client_secret = _appSettings.GitHubClientSecret
            };
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(OAuthGenerateAccessTokenBaseUrl, content);
            var result = response.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<AccessToken>(result);
        }

        public async Task<List<Organization>> GetOrganizations(string username, string accessToken)
        {
            return await _client.GetAsync<List<Organization>>(accessToken, $"/users/{username}/orgs");
        }
        public async Task<IOrganization> GetOrganization(string organization, string accessToken)
        {
            return await _client.GetAsync<Organization>(accessToken, $"/orgs/{organization}");
        }
    }
}
