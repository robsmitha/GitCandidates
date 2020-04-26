using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services.GitHub.Interfaces
{
    public interface IGitHubUser
    {
        string login { get; set; }
        int id { get; set; }
        string node_id { get; set; }
        string avatar_url { get; set; }
        string gravatar_id { get; set; }
        string url { get; set; }
        string html_url { get; set; }
        string followers_url { get; set; }
        string following_url { get; set; }
        string gists_url { get; set; }
        string starred_url { get; set; }
        string subscriptions_url { get; set; }
        string organizations_url { get; set; }
        string repos_url { get; set; }
        string events_url { get; set; }
        string received_events_url { get; set; }
        string type { get; set; }
        bool site_admin { get; set; }
        string name { get; set; }
        object company { get; set; }
        string blog { get; set; }
        object location { get; set; }
        object email { get; set; }
        bool hireable { get; set; }
        string bio { get; set; }
        int public_repos { get; set; }
        int public_gists { get; set; }
        int followers { get; set; }
        int following { get; set; }
        DateTime created_at { get; set; }
        DateTime updated_at { get; set; }
    }
}
