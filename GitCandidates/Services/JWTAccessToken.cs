using Infrastructure.Identity;
using System;

namespace GitCandidates.Services
{
    public class JWTAccessToken : IJWTAccessToken
    {
        public string token_type { get; set; }
        public string access_token { get; set; }
        public DateTime expires_at { get; set; }
    }

}
