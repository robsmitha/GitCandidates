using Domain.Services.Configuration.Interfaces;

namespace Domain.Services.Configuration.Models
{
    public class AppSettings : IAppSettings
    {
        public string JwtSecret { get; set; }
        public string JwtIssuer { get; set; }
        public string GitHubClientID { get; set; }
        public string GitHubClientSecret { get; set; }
        public string GitHubRedirectUrl { get; set; }
        public string GoogleApiKey { get; set; }
    }
}
