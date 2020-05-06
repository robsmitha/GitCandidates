using Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizationPortal.Services
{
    public class JWTAccessToken : IJWTAccessToken
    {
        public string token_type { get; set; }
        public string access_token { get; set; }
        public DateTime expires_at { get; set; }
    }
}
