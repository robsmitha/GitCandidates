using AutoMapper;
using Domain.Services.GitHub.Interfaces;
using Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetOrganizations
{
    public class GetOrganizationsQuery : IRequest<GetOrganizationsResponse>
    {
        public int UserID { get; set; }
        public GetOrganizationsQuery(int userId)
        {
            UserID = userId;
        }
        public class GetOrganizationsQueryHandler : IRequestHandler<GetOrganizationsQuery, GetOrganizationsResponse>
        {
            private readonly IApplicationContext _context;
            private IMapper _mapper;
            private readonly IGitHubService _github;

            public GetOrganizationsQueryHandler(
                IApplicationContext context,
                IMapper mapper,
                IGitHubService github
                )
            {
                _context = context;
                _mapper = mapper;
                _github = github;
            }
            public async Task<GetOrganizationsResponse> Handle(GetOrganizationsQuery request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FindAsync(request.UserID);
                var organizations = await _github.GetOrganizations(user.GitHubLogin, user.GitHubToken);
                var response = new GetOrganizationsResponse(organizations);
                return response;
            }
        }
    }
}
