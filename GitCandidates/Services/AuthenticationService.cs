using System.Linq;
using System.Security.Claims;
using System;
using System.Threading;
using Infrastructure.Identity;
using Microsoft.Extensions.Options;
using Infrastructure.Data;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Logging;
using Domain.Services.GitHub.Interfaces;
using GitCandidates.Services.GitHub.Models;

namespace GitCandidates.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IApplicationContext _context;
        private IAppSettings _appSettings;

        private readonly IIdentityService _identity;

        public AuthenticationService(IOptions<AppSettings> appSettings,
            IIdentityService identity,
            IApplicationContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;
            _identity = identity;
        }

        public async Task<IApplicationUser> AuthorizeUser(IGitHubUser gitHubUser, IAccessToken accessToken, CancellationToken cancellationToken)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.GitHubUsername.ToLower() == gitHubUser.login.ToLower());

            if (user != null)
            {
                user.GitHubToken = accessToken.access_token;    //update token
                user.ModifiedTime = DateTime.Now;
                _context.Users.Update(user);
            }
            else
            {
                var confirmed = _context.UserStatusTypes.FirstOrDefault(s => s.Name == "Submitted");
                user = new User
                {
                    GitHubUsername = gitHubUser.login,
                    UserStatusTypeID = confirmed.ID,
                    GitHubToken = accessToken.access_token
                };
                _context.Users.Add(user);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return new ApplicationUser(IssueJWTToken(user));
        }

        public void ClearAuthentication()
        {
            _identity.SetIdentity();
        }

        public async Task<IApplicationUser> RefreshJWTToken(IJWTAccessToken accessToken)
        {
            try
            {
                var claimsPrincipal = ValidateTokenClaimsPrincipal(accessToken.access_token);
                var id = GetClaimFromPrincipal<int>(claimsPrincipal, ClaimTypes.NameIdentifier);
                if (id != default)
                {
                    var user = await _context.Users.FindAsync(id);

                    if (user?.ID > 0)
                        return new ApplicationUser(IssueJWTToken(user));
                }
            }
            catch (Exception) { }

            _identity.SetIdentity();

            return new ApplicationUser();
        }

        private bool IssueJWTToken(User user)
        {
            try
            {
                var expires = DateTime.UtcNow.AddDays(7);
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtSecret));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[] {
                    new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                    new Claim(ClaimTypes.Email, user.GitHubUsername),
                };

                var token = new JwtSecurityToken(_appSettings.JwtIssuer,
                  _appSettings.JwtIssuer,
                  claims: claims,
                  expires: expires,
                  signingCredentials: credentials);

                var jwtToken = new JwtSecurityTokenHandler()
                    .WriteToken(token);

                var accessToken = new JWTAccessToken
                {
                    access_token = jwtToken,
                    token_type = "",
                    expires_in = expires.ToString()
                };
                var gitHubAccessToken = new AccessToken
                {
                    access_token = user.GitHubToken
                };
                _identity.SetIdentity(accessToken, claims, gitHubAccessToken);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private T GetClaimFromPrincipal<T>(ClaimsPrincipal claimsPrincipal, string claimType)
        {
            var claim = claimsPrincipal?.Claims
                .FirstOrDefault(x => x.Type == claimType);
            return !string.IsNullOrEmpty(claim?.Value) && claim.Value.GetType() != typeof(T)
                ? (T)Convert.ChangeType(claim.Value, typeof(T))
                : default;
        }

        private ClaimsPrincipal ValidateTokenClaimsPrincipal(string jwtToken)
        {
            try
            {
                IdentityModelEventSource.ShowPII = true;

                var validationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = _appSettings.JwtIssuer,
                    ValidIssuer = _appSettings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtSecret))
                };

                var principal = new JwtSecurityTokenHandler()
                    .ValidateToken(jwtToken, validationParameters, out SecurityToken validatedToken);

                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
