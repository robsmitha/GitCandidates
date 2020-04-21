using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services
{
    public interface IGitHubUser
    {
        string login { get; set; }
        string avatar_url { get; set; }
        string url { get; set; }
        string name { get; set; }
        string company { get; set; }
        string blog { get; set; }
        string location { get; set; }
        string email { get; set; }
        string bio { get; set; }
    }
}
