using Domain.Entities;
using Domain.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Authorizes a user
        /// </summary>
        /// <param name="gitHubUser"></param>
        /// <param name="accessToken"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IApplicationUser> AuthorizeUser(IGitHubUser gitHubUser, IGitHubAccessToken accessToken, CancellationToken cancellationToken);

        /// <summary>
        /// Refreshes JWT token cookie when user visits site
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<IApplicationUser> RefreshJWTToken(IJWTAccessToken token);

        /// <summary>
        /// Clears token/session authentication values
        /// </summary>
        void ClearAuthentication();
    }
}
