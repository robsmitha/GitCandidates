using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Jobs.Queries.GetJobs
{
    public class GetJobsQuery : IRequest<List<GetJobsModel>>
    {
        public string Keyword { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double Miles { get; set; }
        public GetJobsQuery(string keyword = null, double? latitude = null, double? longitude = null, int? mi = null)
        {
            Keyword = keyword;
            Latitude = latitude;
            Longitude = longitude;
            Miles = mi ?? 50;
        }
    }
    public class GetJobsQueryHandler : IRequestHandler<GetJobsQuery, List<GetJobsModel>>
    {
        private readonly IApplicationContext _context;
        private IMapper _mapper;

        public GetJobsQueryHandler(
            IApplicationContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<GetJobsModel>> Handle(GetJobsQuery request, CancellationToken cancellationToken)
        {
            var data = from j in _context.Jobs.AsEnumerable()
                       join c in _context.Companies.AsEnumerable() on j.CompanyID equals c.ID
                       join l in _context.JobLocations.AsEnumerable() on j.ID equals l.JobID
                       where j.IsAvailable() && j.HasKeyword(request.Keyword) && l.WithinMiles(request.Latitude, request.Longitude, request.Miles)
                       select new { j, l };

            var map = new Dictionary<int, GetJobsModel>();
            foreach (var d in data)
            {
                if (!map.ContainsKey(d.j.ID))
                    map.Add(d.j.ID, _mapper.Map<GetJobsModel>(d.j));

                map[d.j.ID].Locations.Add(_mapper.Map<JobLocationModel>(d.l));
            }
            await Task.FromResult(0);
            return map.Values.ToList();
        }
    }
}
