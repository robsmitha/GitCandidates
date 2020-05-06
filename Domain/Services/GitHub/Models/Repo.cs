using Domain.Services.GitHub.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services.GitHub.Models
{
    public class Repo : IRepo
    {
        public int id { get; set; }
        public string url { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string language { get; set; }
    }
}
