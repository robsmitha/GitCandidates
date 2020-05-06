using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Jobs.Commands.CreateJobApplication;
using Application.Jobs.Queries.GetJob;
using Application.Jobs.Queries.GetJobApplication;
using Application.Jobs.Queries.SearchJobs;
using Application.Jobs.Queries.GetJobSkills;
using Domain.Services.GitHub.Interfaces;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitCandidates.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IGitHubService _github;
        private readonly IIdentityService _identity;
        public JobsController(IMediator mediator, IIdentityService identity, IGitHubService github)
        {
            _identity = identity;
            _github = github;
            _mediator = mediator;
        }

        [HttpPost("SearchJobs")]
        public async Task<ActionResult<SearchJobsModel>> SearchJobs(SearchJobsRequest request)
        {
            return Ok(await _mediator.Send(new SearchJobsQuery(request)));
        }

        [HttpGet("GetJob/{id}")]
        public async Task<ActionResult<GetJobModel>> GetJob(int id)
        {
            return Ok(await _mediator.Send(new GetJobQuery(id, _identity.ClaimID)));
        }
        [HttpGet("GetJobSkills/{id}")]
        public async Task<ActionResult<List<GetJobSkillModel>>> GetJobSkills(int id)
        {
            return Ok(await _mediator.Send(new GetJobSkillsQuery(id, _identity.ClaimID)));
        }
        [HttpGet("GetJobApplication/{id}")]
        public async Task<ActionResult<GetJobApplicationModel>> GetJobApplication(int id)
        {
            return Ok(await _mediator.Send(new GetJobApplicationQuery(id)));
        }

        [HttpPost("CreateJobApplication")]
        [Authorize]
        public async Task<ActionResult<bool>> CreateJobApplication(CreateJobApplicationModel request)
        {
            return Ok(await _mediator.Send(new CreateJobApplicationCommand(request, _identity.ClaimID)));
        }

    }
}