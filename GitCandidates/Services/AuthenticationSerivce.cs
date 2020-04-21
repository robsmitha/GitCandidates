using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System;
using System.Text.Json;
using System.Threading;
using Infrastructure.Identity;
using Microsoft.Extensions.Options;
using Infrastructure.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Logging;

namespace GitCandidates.Services
{
    public class AuthenticationSerivce : IAuthenticationService
    {
        private readonly IApplicationContext _context;
        private IAppSettings _appSettings;

        private readonly IIdentityService _identity;

        public AuthenticationSerivce(IOptions<AppSettings> appSettings,
            IIdentityService identity,
            IApplicationContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;
            _identity = identity;
        }

        public async Task<IApplicationUser> CreateUser(User model, CancellationToken cancellationToken)
        {
            var user = new User
            {
                GitHubUsername  = model.GitHubUsername
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);
            var auth = IssueToken(user);
            return new ApplicationUser(auth);
        }

        public void ClearAuthentication()
        {
            _identity.SetIdentity();
        }

        public async Task<IApplicationUser> RefreshToken(IAccessToken accessToken)
        {
            try
            {
                var claimsPrincipal = ValidateTokenClaimsPrincipal(accessToken.access_token);
                var id = GetClaimFromPrincipal<int>(claimsPrincipal, ClaimTypes.NameIdentifier);
                if (id != default)
                {
                    var customer = await _context.Users
                        .FindAsync(id);

                    if (customer?.ID > 0)
                        return new ApplicationUser(IssueToken(customer));
                }
            }
            catch (Exception) { }

            _identity.SetIdentity();

            return new ApplicationUser();
        }

        private bool IssueToken(User user)
        {
            try
            {
                var expires = DateTime.UtcNow.AddDays(7);
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));
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

                var accessToken = new AccessToken
                {
                    access_token = jwtToken,
                    token_type = "",
                    expires_in = expires.ToString()
                };

                _identity.SetIdentity(accessToken, claims);

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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret))
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
