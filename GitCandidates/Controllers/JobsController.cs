using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Jobs.Queries.GetJob;
using Application.Jobs.Queries.GetJobs;
using Domain.Services.GitHub.Interfaces;
using Infrastructure.Identity;
using MediatR;
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

        [HttpPost("GetJobs")]
        public async Task<ActionResult<List<GetJobsModel>>> GetJobs(SearchJobsRequest request)
        {
            return Ok(await _mediator.Send(new GetJobsQuery(request.keyword, request.lat, request.lng, request.miles)));
        }

        [HttpGet("GetJob/{id}")]
        public async Task<ActionResult<List<GetJobModel>>> GetJob(int id)
        {
            return Ok(await _mediator.Send(new GetJobQuery(id)));
        }
    }
}