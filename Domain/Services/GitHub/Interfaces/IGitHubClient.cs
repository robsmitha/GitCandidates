using System.Threading.Tasks;

namespace Domain.Services.GitHub.Interfaces
{
    public interface IGitHubClient
    {
        T Get<T>(IAccessToken accessToken, string requestUri, string expansions = null);
        Task<T> GetAsync<T>(IAccessToken accessToken, string requestUri);
    }
}
