using Domain.Services.GitHub.Interfaces;

namespace Domain.Services.GitHub.Models
{
    public class GenerateOAuthAccessToken : IGenerateOAuthAccessToken
    {
        public string code { get; set; }
        public string state { get; set; }
    }
}
