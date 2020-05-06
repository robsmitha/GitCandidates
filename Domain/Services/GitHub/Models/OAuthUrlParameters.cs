using Domain.Services.GitHub.Interfaces;
using System.Text;

namespace Domain.Services.GitHub.Models
{
    public class OAuthUrlParameters : IOAuthUrlParameters
    {
        public string login { get; set; }
        public string state { get; set; }
        public string scope { get; set; }
        public bool allow_signup { get; set; }
        public string client_id { get; set; }
        public string redirect_url { get; set; }
        public OAuthUrlParameters(string login, string state, string scope, bool allow_signup, string client_id, string redirect_url)
        {
            this.login = login;
            this.state = state;
            this.scope = scope;
            this.allow_signup = allow_signup;
            this.client_id = client_id;
            this.redirect_url = redirect_url;
        }
        public string UrlBuilder()
        {
            var sb = new StringBuilder();
            sb.Append(GitHubService.OAuthBaseUrl);
            sb.Append($"allow_signup={allow_signup}");
            sb.Append($"&client_id={client_id}");
            sb.Append($"&redirect_uri={redirect_url}");
            sb.Append($"&login={login}");
            sb.Append($"&state={state}");
            return sb.ToString();
        }
    }
}
