using Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace GitCandidates.Services
{
    public class ApplicationUser : IApplicationUser
    {
        public bool auth { get; set; }
        public ApplicationUser() { }
        public ApplicationUser(bool auth)
        {
            this.auth = auth;
        }
    }
}
