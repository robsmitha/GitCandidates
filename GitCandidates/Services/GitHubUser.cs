using Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitCandidates.Services
{
    public class GitHubUser : IGitHubUser
    {
        public string login { get; set; }
        public string avatar_url { get; set; }
        public string url { get; set; }
        public string name { get; set; }
        public string company { get; set; }
        public string blog { get; set; }
        public string location { get; set; }
        public string email { get; set; }
        public string bio { get; set; }
    }
}
