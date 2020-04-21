using Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Creates and authenticates a new user
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IApplicationUser> CreateUser(User user, CancellationToken cancellationToken);

        /// <summary>
        /// Refreshes JWT token cookie when user visits site
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<IApplicationUser> RefreshToken(IAccessToken token);

        /// <summary>
        /// Clears token/session authentication values
        /// </summary>
        void ClearAuthentication();
    }
}
