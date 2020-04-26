using System.Threading.Tasks;
using Application.Users.Commands.WithdrawApplication;
using Application.Users.GetUser;
using Domain.Services.GitHub.Interfaces;
using GitCandidates.Services.GitHub.Models;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitCandidates.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IIdentityService _identity;
        public UsersController(IIdentityService identity, IMediator mediator)
        {
            _identity = identity;
            _mediator = mediator;
        }

        [HttpGet("GetUser")]
        public async Task<ActionResult<GetUserModel>> GetUser()
        {
            return Ok(await _mediator.Send(new GetUserQuery(_identity.ClaimID)));
        }

        [HttpPost("WithdrawApplication")]
        public async Task<ActionResult<bool>> WithdrawApplication(WithdrawApplicationModel model)
        {
            return Ok(await _mediator.Send(new WithdrawApplicationCommand(model.ID, _identity.ClaimID)));
        }
    }
}