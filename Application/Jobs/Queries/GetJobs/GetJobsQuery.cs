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
        public int? WithinXXMiles { get; set; }
        public GetJobsQuery(string keyword = null, double? latitude = null, double? longitude = null, int? withinXXMiles = null)
        {
            Keyword = keyword;
            Latitude = latitude;
            Longitude = longitude;
            WithinXXMiles = withinXXMiles ?? 50;
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
            var jobs = await _context.Jobs
                .Where(j => j.Active)
                .ToListAsync();

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                jobs = jobs
                    .Where(j => j.Name.ToLower().Contains(request.Keyword.ToLower()) 
                    || j.Description.ToLower().Contains(request.Keyword.ToLower()))
                    .ToList();
            }

            if(request.Latitude != null && request.Longitude != null)
            {
                jobs = jobs.Where(j => calculate(request.Latitude.Value, request.Longitude.Value, j.Latitide, j.Longitude) <= request.WithinXXMiles)  //todo: convert to miles
                    .ToList();
            }

            return _mapper.Map<List<GetJobsModel>>(jobs);
        }
        public static double calculate(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6372.8; // In kilometers
            var dLat = toRadians(lat2 - lat1);
            var dLon = toRadians(lon2 - lon1);
            lat1 = toRadians(lat1);
            lat2 = toRadians(lat2);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
            var c = 2 * Math.Asin(Math.Sqrt(a));
            return R * c;
        }

        public static double toRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }
    }
}
