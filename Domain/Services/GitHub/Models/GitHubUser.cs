using Domain.Services.GitHub.Attributes;
using Domain.Services.GitHub.Interfaces;
using System;
using System.Collections.Generic;

namespace Domain.Services.GitHub.Models
{
    public class GitHubUser : IGitHubUser
    {
        public GitHubUser()
        {
            organizations = new List<Organization>();
            repos = new List<Repo>();
        }
        public string login { get; set; }
        public int id { get; set; }
        public string node_id { get; set; }
        public string avatar_url { get; set; }
        public string gravatar_id { get; set; }
        public string url { get; set; }
        public string html_url { get; set; }
        public string followers_url { get; set; }
        public string following_url { get; set; }
        public string gists_url { get; set; }
        public string starred_url { get; set; }
        public string subscriptions_url { get; set; }
        [Expandable(Expand = "organizations", Result = typeof(List<Organization>), MapTo = "organizations")]
        public string organizations_url { get; set; }
        [Expandable(Expand = "repos", Result = typeof(List<Repo>), MapTo = "repos")]
        public string repos_url { get; set; }
        public string events_url { get; set; }
        public string received_events_url { get; set; }
        public string type { get; set; }
        public bool site_admin { get; set; }
        public string name { get; set; }
        public object company { get; set; }
        public string blog { get; set; }
        public object location { get; set; }
        public object email { get; set; }
        public bool hireable { get; set; }
        public string bio { get; set; }
        public int public_repos { get; set; }
        public int public_gists { get; set; }
        public int followers { get; set; }
        public int following { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

        //expansions
        public List<Organization> organizations { get; set; }
        public List<Repo> repos { get; set; }
    }
}
