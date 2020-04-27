using System.Threading.Tasks;
using Application.Users.Commands.SetSavedJob;
using Application.Users.Commands.WithdrawApplication;
using Application.Users.Queries.GetJobApplications;
using Application.Users.Queries.GetSavedJobs;
using Application.Users.Queries.GetUser;
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

        [HttpGet("GetJobApplications")]
        public async Task<ActionResult<GetJobApplicationsModel>> GetJobApplications()
        {
            return Ok(await _mediator.Send(new GetJobApplicationsQuery(_identity.ClaimID)));
        }

        [HttpGet("GetSavedJobs")]
        public async Task<ActionResult<GetUserModel>> GetSavedJobs()
        {
            return Ok(await _mediator.Send(new GetSavedJobsQuery(_identity.ClaimID)));
        }

        [HttpPost("WithdrawApplication")]
        public async Task<ActionResult<bool>> WithdrawApplication(WithdrawApplicationModel model)
        {
            return Ok(await _mediator.Send(new WithdrawApplicationCommand(model.ID, _identity.ClaimID)));
        }

        [HttpPost("SetSavedJob")]
        public async Task<ActionResult<bool>> SetSavedJob(SetSavedJobModel model)
        {
            return Ok(await _mediator.Send(new SetSavedJobCommand(model.JobID, _identity.ClaimID, model.SavedJobID)));
        }
    }
}