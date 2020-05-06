using Domain.Services.GitHub.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services.GitHub.Models
{
    public class Organization : IOrganization
    {
        public int id { get; set; }
        public string url { get; set; }
        public string login { get; set; }
        public string description { get; set; }
        public string avatar_url { get; set; }
    }
}
