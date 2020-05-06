using Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationPortal.Services
{
    public class ApplicationUser : IApplicationUser
    {
        public bool auth { get; set; }
        public string GitHubLogin { get; set; }
        public ApplicationUser() { }
        public ApplicationUser(bool auth, string gitHubLogin = null)
        {
            this.auth = auth;
            GitHubLogin = gitHubLogin;
        }
    }
}
