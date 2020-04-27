using AutoMapper;
using Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetSavedJobs
{
    public class GetSavedJobsQuery : IRequest<List<GetSavedJobsModel>>
    {
        public int UserID { get; set; }
        public GetSavedJobsQuery(int userId)
        {
            UserID = userId;
        }
    }
    public class GetSavedJobsQueryHandler : IRequestHandler<GetSavedJobsQuery, List<GetSavedJobsModel>>
    {
        private readonly IApplicationContext _context;
        private readonly IMapper _mapper;
        public GetSavedJobsQueryHandler(IApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<GetSavedJobsModel>> Handle(GetSavedJobsQuery request, CancellationToken cancellationToken)
        {
            var data = from sj in _context.SavedJobs.AsEnumerable()
                       join j in _context.Jobs.AsEnumerable() on sj.JobID equals j.ID
                       join c in _context.Companies.AsEnumerable() on j.CompanyID equals c.ID
                       where sj.UserID == request.UserID
                       select sj;
            if (data == null || data.FirstOrDefault() == null) return new List<GetSavedJobsModel>();
            var model = _mapper.Map<List<GetSavedJobsModel>>(data.AsEnumerable());
            await Task.FromResult(0);
            return model;
        }
    }
}
