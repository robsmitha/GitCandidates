using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Jobs.Queries.GetJob
{
    public class GetJobQuery : IRequest<GetJobModel>
    {
        public int JobID { get; set; }
        public int UserID { get; set; }
        public GetJobQuery(int jobId, int userId)
        {
            JobID = jobId;
            UserID = userId;
        }
    }
    public class GetJobQueryHandler : IRequestHandler<GetJobQuery, GetJobModel>
    {
        private readonly IApplicationContext _context;
        private IMapper _mapper;

        public GetJobQueryHandler(
            IApplicationContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<GetJobModel> Handle(GetJobQuery request, CancellationToken cancellationToken)
        {
            var data = from j in _context.Jobs.AsEnumerable()
                       join c in _context.Companies.AsEnumerable() on j.CompanyID equals c.ID
                       join l in _context.JobLocations.AsEnumerable() on j.ID equals l.JobID
                       join ja in _context.JobApplications.AsEnumerable() on new { JobID = j.ID, request.UserID} equals new { ja.JobID, ja.UserID } into tmp_ja
                       from ja in tmp_ja.DefaultIfEmpty()
                       join jast in _context.JobApplicationStatusTypes.AsEnumerable() on ja?.JobApplicationStatusTypeID equals jast.ID into tmp_jast
                       from jast in tmp_jast.DefaultIfEmpty()
                       where j.ID == request.JobID
                       select new { j, l, ja };

            if (data?.FirstOrDefault() == null) return new GetJobModel();
            var job = data.First().j;
            var jobApp = data.LastOrDefault().ja;

            var model = _mapper.Map<GetJobModel>(job);
            foreach (var d in data)
                model.Locations.Add(_mapper.Map<JobLocationModel>(d.l));

            model.UserCanApply = jobApp == null || !jobApp.IsActiveApplication();
            model.IsAcceptingApplications = job.IsAcceptingApplications();
            await Task.FromResult(0);
            return model;
        }
    }
}
