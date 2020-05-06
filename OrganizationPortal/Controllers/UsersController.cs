using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Users.Queries.GetOrganization;
using Application.Users.Queries.GetOrganizations;
using Application.Users.Queries.GetUser;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OrganizationPortal.Controllers
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

        [HttpGet("GetOrganizations")]
        public async Task<ActionResult<GetOrganizationsResponse>> GetOrganizations()
        {
            return Ok(await _mediator.Send(new GetOrganizationsQuery(_identity.ClaimID)));
        }

        [HttpGet("GetOrganization/{org}")]
        public async Task<ActionResult<GetOrganizationResponse>> GetOrganization(string org)
        {
            return Ok(await _mediator.Send(new GetOrganizationQuery(_identity.ClaimID, org)));
        }
    }
}