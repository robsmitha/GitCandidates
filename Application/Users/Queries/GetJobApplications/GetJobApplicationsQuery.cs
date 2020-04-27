using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetJobApplications
{
    public class GetJobApplicationsQuery : IRequest<GetJobApplicationsModel>
    {
        public int UserID { get; set; }
        public GetJobApplicationsQuery(int userId)
        {
            UserID = userId;
        }
    }
    public class GetJobApplicationsQueryHandler : IRequestHandler<GetJobApplicationsQuery, GetJobApplicationsModel>
    {
        private readonly IApplicationContext _context;
        private readonly IMapper _mapper;
        public GetJobApplicationsQueryHandler(IApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<GetJobApplicationsModel> Handle(GetJobApplicationsQuery request, CancellationToken cancellationToken)
        {
            var data = from ja in _context.JobApplications.AsEnumerable()
                       join j in _context.Jobs.AsEnumerable() on ja.JobID equals j.ID
                       join c in _context.Companies.AsEnumerable() on j.CompanyID equals c.ID
                       join jast in _context.JobApplicationStatusTypes.AsEnumerable() on ja.JobApplicationStatusTypeID equals jast.ID
                       where ja.UserID == request.UserID
                       select ja;

            if (data == null || data.FirstOrDefault() == null) return new GetJobApplicationsModel();

            var active = from a in data.AsEnumerable()
                         where a.IsActiveApplication()
                         select _mapper.Map<JobApplicationModel>(a);

            var inactive = from a in data.AsEnumerable()
                         where !a.IsActiveApplication()
                         select _mapper.Map<JobApplicationModel>(a);

            var model = new GetJobApplicationsModel
            {
                JobApplications = active.ToList(),
                InactiveJobApplications = inactive.ToList()
            };
            await Task.FromResult(0);
            return model;
        }
    }
}
