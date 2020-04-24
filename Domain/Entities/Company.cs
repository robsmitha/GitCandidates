using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Company : BaseEntity
    {
        /// <summary>
        /// GitHub Organization login
        /// </summary>
        public string GitHubLogin { get; set; }
        public string GitHubAccessToken { get; set; }
    }
}
