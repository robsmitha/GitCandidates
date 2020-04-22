using Domain.Services.GitHub.Attributes;
using Domain.Services.GitHub.Interfaces;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace CleanersNextDoor.Services.GitHub
{
    public class GitHubClient : IGitHubClient
    {
        public const string ApiBaseUrl = "https://api.github.com";

        public T Send<T>(IAccessToken accessToken, string requestUri)
        {
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            client.DefaultRequestHeaders.Add("Authorization", $"token {accessToken.access_token}");
            client.DefaultRequestHeaders.Add("User-Agent", "GitCandidates");
            var response = client.GetAsync(FormatRequestUri(requestUri)).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<T>(result);
        }
        public T Get<T>(IAccessToken accessToken, string requestUri, string expansionKeys = null)
        {
            //Generic result
            var result = Send<T>(accessToken, requestUri);

            if (!string.IsNullOrEmpty(expansionKeys))
            {
                //Get expandable properties from result object
                var expandables = ExpandableAttribute.MapFromObject(result);
                if(expandables.Count != 0)
                {
                    //get Send<> method info for creating generic methods
                    var mSend = typeof(GitHubClient).GetMethod("Send");

                    //split array of requested expansions
                    expansionKeys.Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .ToList()
                        .ForEach(e =>
                        {
                            if (expandables.ContainsKey(e))
                            {
                                //init Send<> method w/ generic type
                                var fnSend = mSend.MakeGenericMethod(new[] { expandables[e].genericType });

                                //call Send<> to get response
                                var value = fnSend.Invoke(new GitHubClient(), 
                                    new object[] { accessToken, expandables[e].value });

                                //map response value to property
                                var mapToProp = result.GetType().GetProperty(expandables[e].mapTo);
                                mapToProp.SetValue(result, Convert.ChangeType(value, mapToProp.PropertyType), null);
                            }
                            else
                                throw new Exception($"Missing expandable custom attribute configuration for {e}");
                        });
                }
            }

            return result;
        }
        private async Task<T> SendAsync<T>(IAccessToken accessToken, string requestUri)
        {
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            client.DefaultRequestHeaders.Add("Authorization", $"token {accessToken.access_token}");
            client.DefaultRequestHeaders.Add("User-Agent", "GitCandidates");
            var response = await client.GetAsync(FormatRequestUri(requestUri));
            var result = response.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<T>(result);
        }
        public async Task<T> GetAsync<T>(IAccessToken accessToken, string requestUri)
        {
            return await SendAsync<T>(accessToken, ApiBaseUrl + requestUri);
        }
        private string FormatRequestUri(string requestUri)
        {
            if (!requestUri.StartsWith(ApiBaseUrl))
                return ApiBaseUrl + (!requestUri.StartsWith("/") ? $"/{requestUri}" : requestUri);

            return requestUri; //full api call
        }
    }
}
