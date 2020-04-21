using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IGitHubAccessToken
    {
        string token_type { get; set; }
        string access_token { get; set; }
        string scope { get; set; }
    }
}
