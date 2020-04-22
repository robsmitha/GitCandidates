using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Identity
{
    public class IdentityConstants
    {
        public const string AUTHENTICATED_SESSION_KEY = "auth";
        public const string JWT_ACCESS_TOKEN_COOKIE_KEY = "access_token";
        public const string OAUTH_STATE_SESSION_KEY = "state";
        public const string GITHUB_ACCESS_TOKEN = "github_access_token";
    }
}
