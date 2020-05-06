namespace Domain.Services.Configuration.Interfaces
{
    public interface IAppSettings
    {
        string JwtSecret { get; set; }
        string JwtIssuer { get; set; }
        string GitHubClientID { get; set; }
        string GitHubClientSecret { get; set; }
        string GitHubRedirectUrl { get; set; }
        string GoogleApiKey { get; set; }
    }
}