using System.Threading.Tasks;

namespace Domain.Services.GitHub.Interfaces
{
    public interface IGitHubClient
    {
        T Get<T>(string accessToken, string requestUri, string expansions = null);
        Task<T> GetAsync<T>(string accessToken, string requestUri, string expansionKeys = null);
    }
}
