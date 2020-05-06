using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Domain.Services.Configuration.Interfaces;
using Domain.Services.Configuration.Models;
using Domain.Services.GitHub.Interfaces;
using Domain.Services.GitHub.Models;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OrganizationPortal.Common;
using OrganizationPortal.Services;

namespace OrganizationPortal.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAppSettings _appSettings;
        private readonly IGitHubService _github;
        private readonly IAuthenticationService _auth;
        public AuthController(IOptions<AppSettings> appSettings, IAuthenticationService auth, IGitHubService github)
        {
            _appSettings = appSettings.Value;
            _auth = auth;
            _github = github;
        }

        [HttpGet("GitHubOAuthUrl/{login}")]
        [AllowAnonymous]
        public dynamic GitHubOAuthUrl(string login)
        {
            var state = Guid.NewGuid().ToString();
            HttpContext.Session.Set(IdentityConstants.OAUTH_STATE_SESSION_KEY, state);
            var OAuth = new OAuthUrlParameters(
                login: login,
                state: state,
                scope: "user repo",
                allow_signup: true,
                client_id: _appSettings.GitHubClientID,
                redirect_url: _appSettings.GitHubRedirectUrl);
            return new
            {
                url = OAuth.UrlBuilder()
            };
        }

        [HttpPost("GitHubOAuthCallback")]
        [AllowAnonymous]
        public async Task<ActionResult<IApplicationUser>> GitHubOAuthCallback(GenerateOAuthAccessToken authParams)
        {
            var state = HttpContext.Session.Get<string>(IdentityConstants.OAUTH_STATE_SESSION_KEY);
            if (state != authParams.state) return BadRequest("Failed CSRF protection check.");
            var cancellationToken = new CancellationToken();
            var accessToken = await _github.GenerateOAuthAccessToken(authParams);
            HttpContext.Session.Remove(IdentityConstants.OAUTH_STATE_SESSION_KEY);
            var gitHubUser = await _github.GetAuthenticatedUser(accessToken.access_token);
            return Ok(await _auth.AuthorizeUser(gitHubUser, accessToken, cancellationToken));
        }

        [HttpPost("Authorize")]
        [AllowAnonymous]
        public async Task<IApplicationUser> Authorize()
        {
            var token = HttpContext.Request.Cookies[IdentityConstants.JWT_ACCESS_TOKEN_COOKIE_KEY];
            var authenticated = HttpContext.Session.Get<bool>(IdentityConstants.AUTHENTICATED_SESSION_KEY);
            if (!authenticated && token != null)
            {
                var accessToken = JsonSerializer.Deserialize<JWTAccessToken>(token);
                if (accessToken.expires_at > DateTime.Now)
                {
                    return await _auth.RefreshJWTToken(accessToken);
                }
            }
            return new ApplicationUser(authenticated);
        }

        [HttpPost("SignOut")]
        public ActionResult<bool> SignOut()
        {
            _auth.ClearAuthentication();
            return true;
        }
    }
}