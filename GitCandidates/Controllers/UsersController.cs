using System.Threading.Tasks;
using CleanersNextDoor.Common;
using Domain.Services.GitHub.Interfaces;
using GitCandidates.Services;
using GitCandidates.Services.GitHub.Models;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GitCandidates.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IGitHubService _github;
        private readonly IIdentityService _identity;
        public UsersController(IIdentityService identity, IGitHubService github)
        {
            _identity = identity;
            _github = github;
        }

        [HttpGet("GetUser")]
        public async Task<ActionResult<GitHubUser>> GetUser()
        {
            return Ok(await _github.GetUser(_identity.GitHubUsername, _identity.GitHubAccessToken, "repos"));
        }
    }
}