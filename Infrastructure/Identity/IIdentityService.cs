using Domain.Services.GitHub.Interfaces;
using System.Security.Claims;

namespace Infrastructure.Identity
{
    public interface IIdentityService
    {
        /// <summary>
        /// CustomerID or UserID of current authenticated app user
        /// </summary>
        int ClaimID { get; }

        /// <summary>
        /// GitHubUsername of current authenticated app user
        /// </summary>
        string GitHubUsername { get; }

        /// <summary>
        /// Gets access token of authenticated user
        /// </summary>
        IAccessToken GitHubAccessToken { get; }

        /// <summary>
        /// Sets/Replaces identity/authentication values in Cookies, Session and Claims
        /// </summary>
        /// <param name="accessToken">jwt access token</param>
        /// <param name="claims">claims to refresh the current principle</param>
        /// <param name="gitHubAccessToken">github access token</param>
        void SetIdentity(IJWTAccessToken accessToken = null, Claim[] claims = null, IAccessToken gitHubAccessToken = null);
    }
}
