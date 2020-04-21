using Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitCandidates.Services
{
    public class AppSettings : IAppSettings
    {
        public string JwtSecret { get; set; }
        public string JwtIssuer { get; set; }
        public string GitHubClientID { get; set; }
        public string GitHubClientSecret { get; set; }
        public string GitHubRedirectUrl { get; set; }
    }
}
