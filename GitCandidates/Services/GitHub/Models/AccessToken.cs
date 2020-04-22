using Domain.Services.GitHub.Interfaces;

namespace GitCandidates.Services.GitHub.Models
{
    public class AccessToken : IAccessToken
    {
        public string token_type { get; set; }
        public string access_token { get; set; }
        public string scope { get; set; }
    }
}
