using Domain.Services.GitHub.Attributes;
using Domain.Services.GitHub.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Domain.Services.GitHub
{
    public class GitHubClient : IGitHubClient
    {
        public const string ApiBaseUrl = "https://api.github.com";

        public async Task<T> GetAsync<T>(string accessToken, string requestUri, string expansionKeys = null)
        {
            var result = await SendAsync<T>(accessToken, requestUri);

            if (!string.IsNullOrEmpty(expansionKeys))
            {
                //Get expandable properties from result object
                var expandables = result.GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .ToExpandableDictionary(result);

                if (expandables.Count != 0)
                {
                    //get SendAsync<> method info for creating generic methods
                    var mSendAsync = typeof(GitHubClient).GetMethod("SendAsync");

                    //split array of requested expansions
                    expansionKeys.Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .ToList()
                        .ForEach(e =>
                        {
                            if (expandables.ContainsKey(e))
                            {
                                //init SendAsync<> method w/ generic type
                                var fnSend = mSendAsync.MakeGenericMethod(new[] { expandables[e].genericType });

                                //call SendAsync<> with async extension to get response
                                var value = fnSend.InvokeAsync(new GitHubClient(),
                                    new object[] { accessToken, expandables[e].value });

                                //map response value to property
                                result.GetType()
                                .GetProperty(expandables[e].mapTo)
                                ?.SetPropertyValue(ref result, value);
                            }
                            else
                                throw new Exception($"Missing expandable custom attribute configuration for {e}");
                        });
                }
            }

            return result;
        }
        public T Get<T>(string accessToken, string requestUri, string expansionKeys = null)
        {
            //Generic result
            var result = Send<T>(accessToken, requestUri);

            if (!string.IsNullOrEmpty(expansionKeys))
            {
                //Get expandable properties from result object
                var expandables = result.GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .ToExpandableDictionary(result);
                if (expandables.Count != 0)
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
                                result.GetType()
                                .GetProperty(expandables[e].mapTo)
                                ?.SetPropertyValue(ref result, value);
                            }
                            else
                                throw new Exception($"Missing expandable custom attribute configuration for {e}");
                        });
                }
            }

            return result;
        }
        public T Send<T>(string accessToken, string requestUri)
        {
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            client.DefaultRequestHeaders.Add("Authorization", $"token {accessToken}");
            client.DefaultRequestHeaders.Add("User-Agent", "GitCandidates");
            var response = client.GetAsync(FormatRequestUri(requestUri)).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<T>(result);
        }

        public async Task<T> SendAsync<T>(string accessToken, string requestUri)
        {
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            client.DefaultRequestHeaders.Add("Authorization", $"token {accessToken}");
            client.DefaultRequestHeaders.Add("User-Agent", "GitCandidates");
            var response = await client.GetAsync(FormatRequestUri(requestUri));
            var result = response.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<T>(result);
        }
        private string FormatRequestUri(string requestUri)
        {
            if (!requestUri.StartsWith(ApiBaseUrl))
                return ApiBaseUrl + (!requestUri.StartsWith("/") ? $"/{requestUri}" : requestUri);

            return requestUri; //full api call
        }
    }
    public static class ReflectionExtensions
    {
        public static async Task<object> InvokeAsync(this MethodInfo @this, object obj, params object[] parameters)
        {
            dynamic awaitable = @this.Invoke(obj, parameters);
            await awaitable;
            return awaitable.GetAwaiter().GetResult();
        }
        /// <summary>
        /// Get expandable properties from an object by filtering class properties
        /// </summary>
        /// <param name="obj">object whose attributes to check</param>
        /// <returns></returns>
        public static Dictionary<string, ExpandableAttribute.MetaData> ToExpandableDictionary(this PropertyInfo[] @this, object obj)
        {
            return @this
                    .Where(prop => Attribute.IsDefined(prop, typeof(ExpandableAttribute)))
                    .Select(prop =>
                    {
                        var attr = prop.GetCustomAttribute(typeof(ExpandableAttribute)) as ExpandableAttribute;
                        return new ExpandableAttribute.MetaData
                        {
                            expand = attr.Expand,
                            genericType = attr.Result,
                            value = prop.GetValue(obj, null),
                            mapTo = attr.MapTo
                        };
                    })
                    .ToDictionary(data => data.expand, data => data);
        }
        public static void SetPropertyValue<T>(this PropertyInfo @this, ref T obj, object value)
        {
            if (@this == null) throw new Exception($"MapTo configuration not found.");
            @this.SetValue(obj, Convert.ChangeType(value, @this.PropertyType), null);
        }
    }
}
