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

        public async Task<User> FindOrCreateUser(IGitHubUser gitHubUser, CancellationToken cancellationToken)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.GitHubLogin.ToLower() == gitHubUser.login.ToLower());

            if (user == null)
            {
                var submitted = _context.UserStatusTypes.FirstOrDefault(s => s.Name == "Submitted");
                user = new User
                {
                    GitHubLogin = gitHubUser.login,
                    UserStatusTypeID = submitted.ID
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return user;
        }

        public async Task<IApplicationUser> AuthorizeUser(IGitHubUser gitHubUser, IAccessToken gitHubAccessToken, CancellationToken cancellationToken)
        {
            var user = await FindOrCreateUser(gitHubUser, cancellationToken);
            return new ApplicationUser(IssueJWTToken(user, gitHubAccessToken.access_token));
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

                    //ensure user is who they say they are
                    if (user?.ID > 0 && user.JWT == accessToken.access_token 
                        && !string.IsNullOrEmpty(user.GitHubToken))    //todo: check token w github.com
                    {
                        return new ApplicationUser(IssueJWTToken(user, user.GitHubToken));
                    }
                        
                }
            }
            catch (Exception) { }

            _identity.SetIdentity();

            return new ApplicationUser();
        }

        private bool IssueJWTToken(User user, string gitHubAccessToken)
        {
            try
            {
                var expires = DateTime.UtcNow.AddDays(7);
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtSecret));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var _claims = new[] {
                    new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                    new Claim(ClaimTypes.Email, user.GitHubLogin),
                };

                var token = new JwtSecurityToken(_appSettings.JwtIssuer,
                  _appSettings.JwtIssuer,
                  claims: _claims,
                  expires: expires,
                  signingCredentials: credentials);


                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                var _accessToken = new JWTAccessToken
                {
                    access_token = jwt,
                    token_type = "",
                    expires_at = expires
                };

                var _gitHubAccessToken = new AccessToken
                {
                    access_token = gitHubAccessToken
                };

                user.JWT = _accessToken.access_token;
                user.GitHubToken = _gitHubAccessToken.access_token;
                user.ModifiedTime = DateTime.Now;
                _context.Users.Update(user);
                _context.SaveChanges();

                _identity.SetIdentity(_accessToken, _claims, _gitHubAccessToken);

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
