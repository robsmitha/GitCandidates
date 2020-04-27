using AutoMapper;
using Domain.Services.GitHub.Interfaces;
using Infrastructure.Data;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetUser
{
    public class GetUserQuery : IRequest<GetUserModel>
    {
        public int UserID { get; set; }
        public GetUserQuery(int userId)
        {
            UserID = userId;
        }
    }
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserModel>
    {
        private readonly IApplicationContext _context;
        private IMapper _mapper;
        private readonly IGitHubService _github;

        public GetUserQueryHandler(
            IApplicationContext context,
            IMapper mapper, 
            IGitHubService github
            )
        {
            _context = context;
            _mapper = mapper;
            _github = github;
        }
        public async Task<GetUserModel> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var data = from u in _context.Users.AsEnumerable()
                       join ust in _context.UserStatusTypes.AsEnumerable() on u.UserStatusTypeID equals ust.ID
                       where u.ID == request.UserID
                       select u;
            if (data == null || data.FirstOrDefault() == null) return new GetUserModel();
            var user = data.First();
            var gUser = await _github.GetUser(user.GitHubLogin, user.GitHubToken);
            var model = _mapper.Map<GetUserModel>(user);
            model.GitHubUser = _mapper.Map<GitHubUserModel>(gUser);
            return model;
        }
    }
}
