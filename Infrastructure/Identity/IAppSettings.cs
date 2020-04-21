namespace Infrastructure.Identity
{
    public interface IAppSettings
    {
        string JwtSecret { get; set; }
        string JwtIssuer { get; set; }
        string GitHubClientID { get; set; }
        string GitHubClientSecret { get; set; }
        string GitHubRedirectUrl { get; set; }
    }
}