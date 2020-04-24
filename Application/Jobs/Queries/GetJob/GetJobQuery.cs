using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Jobs.Queries.GetJob
{
    public class GetJobQuery : IRequest<GetJobModel>
    {
        public int JobID { get; set; }
        public GetJobQuery(int jobId)
        {
            JobID = jobId;
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
                       where j.ID == request.JobID && j.IsAvailable()
                       select new { j, l };

            if (data?.FirstOrDefault() == null) return new GetJobModel();

            var job = _mapper.Map<GetJobModel>(data.FirstOrDefault().j);
            foreach (var d in data)
                job.Locations.Add(_mapper.Map<JobLocationModel>(d.l));

            await Task.FromResult(0);
            return job;
        }
    }
}
