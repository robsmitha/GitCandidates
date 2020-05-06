using AutoMapper;
using Domain.Services.GitHub.Interfaces;
using Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetOrganization
{
    public class GetOrganizationQuery : IRequest<GetOrganizationResponse>
    {
        public int UserID { get; set; }
        public string OrganizationName { get; set; }
        public GetOrganizationQuery(int userId, string organizationName)
        {
            UserID = userId;
            OrganizationName = organizationName;
        }
        public class GetOrganizationQueryHandler : IRequestHandler<GetOrganizationQuery, GetOrganizationResponse>
        {
            private readonly IApplicationContext _context;
            private readonly IGitHubService _github;
            private readonly IMapper _mapper;
            public GetOrganizationQueryHandler(
                IApplicationContext context,
                IGitHubService github,
                IMapper mapper
                )
            {
                _context = context;
                _github = github;
                _mapper = mapper;
            }
            public async Task<GetOrganizationResponse> Handle(GetOrganizationQuery request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FindAsync(request.UserID);
                var organization = await _github.GetOrganization(request.OrganizationName, user.GitHubToken);
                var source = _mapper.Map<GitHubOrganizationModel>(organization);
                var response = new GetOrganizationResponse(source);
                return response;
            }
        }
    }
}
