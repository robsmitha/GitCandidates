using Domain.Entities;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

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
        /// Sets/Replaces identity/authentication values in Cookies, Session and Claims
        /// </summary>
        /// <param name="accessToken">jwt access token</param>
        /// <param name="claims">claims to refresh the current principle</param>
        void SetIdentity(IJWTAccessToken accessToken = null, Claim[] claims = null);
    }
}
