using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services.GitHub.Interfaces
{
    public interface IOrganization
    {
        int id { get; set; }
        string url { get; set; }
        string login { get; set; }
        string description { get; set; }
        string avatar_url { get; set; }
    }
}
