using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Company : BaseType
    {
        /// <summary>
        /// GitHub Organization login
        /// </summary>
        public string GitHubLogin { get; set; }
        public string GitHubAccessToken { get; set; }
        public string Founded { get; set; }
        public string Size { get; set; }
        /// <summary>
        /// HQ location
        /// </summary>
        public string Location { get; set; }
    }
}
