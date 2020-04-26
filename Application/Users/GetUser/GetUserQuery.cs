using AutoMapper;
using Domain.Entities;
using Domain.Services.GitHub.Interfaces;
using Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.GetUser
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
                       join ja in _context.JobApplications.AsEnumerable() on u.ID equals ja.UserID into tmp_ja 
                       from ja in tmp_ja.DefaultIfEmpty()
                       join j in _context.Jobs.AsEnumerable() on ja?.JobID equals j.ID into tmp_j 
                       from j in tmp_j.DefaultIfEmpty()
                       join c in _context.Companies.AsEnumerable() on j?.CompanyID equals c.ID into tmp_c 
                       from c in tmp_c.DefaultIfEmpty()
                       join jast in _context.JobApplicationStatusTypes.AsEnumerable() on ja?.JobApplicationStatusTypeID equals jast.ID into tmp_jast 
                       from jast in tmp_jast.DefaultIfEmpty()
                       where u.ID == request.UserID
                       select new { 
                           u, 
                           ja 
                       };

            if (data == null || data.FirstOrDefault() == null) return new GetUserModel();
            var row = data.First();
            var model = _mapper.Map<GetUserModel>(row.u);

            var gUser = await _github.GetUser(row.u.GitHubLogin, row.u.GitHubToken);
            model.GitHubUser = _mapper.Map<GitHubUserModel>(gUser);

            foreach (var r in data.Where(d => d.ja != null))
            {
                var ja = _mapper.Map<JobApplicationModel>(r.ja);
                if (r.ja.IsActiveApplication())
                    model.JobApplications.Add(ja);
                else
                    model.InactiveJobApplications.Add(ja);
            }

            return model;
        }
    }
}
