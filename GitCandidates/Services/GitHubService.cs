using Domain.Services;
using Infrastructure.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GitCandidates.Services
{
    public class GitHubService : IGitHubService
    {
        public const string ApiBaseUrl = "https://api.github.com";
        public const string OAuthBaseUrl = "https://github.com/login/oauth/authorize?";
        public const string OAuthGenerateAccessTokenBaseUrl = "https://github.com/login/oauth/access_token";

        private readonly IAppSettings _appSettings;
        public GitHubService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task<IGitHubAccessToken> GenerateOAuthAccessToken(IGenerateOAuthAccessToken generateOAuthAccessToken)
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
            return JsonSerializer.Deserialize<GitHubAccessToken>(result);
        }

        public async Task<IGitHubUser> GetAuthenticatedUser(IGitHubAccessToken accessToken)
        {
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            client.DefaultRequestHeaders.Add("Authorization", $"token {accessToken.access_token}");
            client.DefaultRequestHeaders.Add("User-Agent", "GitCandidates");
            var requestUri = $"{ApiBaseUrl}/user";
            var response = await client.GetAsync(requestUri);
            var result = response.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<GitHubUser>(result);
        }
    }
}
