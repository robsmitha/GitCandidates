using CleanersNextDoor.Services.GitHub.Models;
using Domain.Services.GitHub.Attributes;
using Domain.Services.GitHub.Interfaces;
using System.Collections.Generic;

namespace GitCandidates.Services.GitHub.Models
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
        [Expandable(Expand = "organizations", Result = typeof(List<Organization>), MapTo = "organizations")]
        public string organizations_url { get; set; }
        public List<Organization> organizations { get; set; }

        [Expandable(Expand = "repos", Result = typeof(List<Repo>), MapTo = "repos")]
        public string repos_url { get; set; }
        public List<Repo> repos { get; set; }
    }
}
