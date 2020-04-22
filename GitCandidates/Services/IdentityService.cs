using CleanersNextDoor.Common;
using Domain.Services.GitHub.Interfaces;
using GitCandidates.Services.GitHub.Models;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;

namespace CleanersNextDoor.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Unique int PK of current user
        /// </summary>
        public int ClaimID => int.TryParse(
            _httpContextAccessor?
            .HttpContext
            .User
            .Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
            out var @int)
            ? @int
            : 0;

        /// <summary>
        /// Unique string identifier of current user
        /// </summary>
        public string GitHubUsername => _httpContextAccessor?
            .HttpContext
            .User
            .Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

        /// <summary>
        /// Gets access token of authenticated user
        /// </summary>
        public IAccessToken GitHubAccessToken => _httpContextAccessor?
            .HttpContext
            .Session
            .Get<AccessToken>(IdentityConstants.GITHUB_ACCESS_TOKEN);

        public void SetIdentity(IJWTAccessToken accessToken = null, Claim[] claims = null, IAccessToken gitHubAccessToken = null)
        {
            if (_httpContextAccessor?.HttpContext == null) return;

            //remove previous if exists
            _httpContextAccessor
               .HttpContext
               .Response
               .Cookies.Delete(IdentityConstants.JWT_ACCESS_TOKEN_COOKIE_KEY);

            //clear authenticated flag
            _httpContextAccessor
                .HttpContext
                .Session
                .Remove(IdentityConstants.AUTHENTICATED_SESSION_KEY);

            //clear github access token
            _httpContextAccessor
                .HttpContext
                .Session
                .Remove(IdentityConstants.GITHUB_ACCESS_TOKEN);


            if (accessToken != null)
            {
                //add or replace token
                _httpContextAccessor
                   .HttpContext
                   .Response
                   .Cookies.Append(IdentityConstants.JWT_ACCESS_TOKEN_COOKIE_KEY, JsonSerializer.Serialize(accessToken),
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = Convert.ToDateTime(accessToken.expires_in)
                    });

                //reset authenticated flag
                _httpContextAccessor
                    .HttpContext
                    .Session
                    .Set(IdentityConstants.AUTHENTICATED_SESSION_KEY, true);

                if (claims != null)
                {
                    //add to new identity claims
                    var identity = new ClaimsIdentity(claims);
                    var principal = new ClaimsPrincipal(identity);
                    _httpContextAccessor.HttpContext.User = principal;
                    Thread.CurrentPrincipal = principal;
                }

                if(gitHubAccessToken != null)
                {
                    //set github access token for user
                    _httpContextAccessor
                        .HttpContext
                        .Session
                        .Set(IdentityConstants.GITHUB_ACCESS_TOKEN, gitHubAccessToken);
                }
            }
        }
    }
}
