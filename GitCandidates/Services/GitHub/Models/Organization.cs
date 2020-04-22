using Domain.Services.GitHub.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanersNextDoor.Services.GitHub.Models
{
    public class Organization : IOrganization
    {
        public int id { get; set; }
        public string url { get; set; }
        public string name { get; set; }
    }
}
