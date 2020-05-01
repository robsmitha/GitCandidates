using AutoMapper;
using Domain.Entities;
using Domain.Services.GoogleGeocode.Interfaces;
using Domain.Services.GoogleGeocode.Models;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Jobs.Queries.SearchJobs
{
    public class SearchJobsQuery : IRequest<SearchJobsModel>
    {
        public string Keyword { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double Miles { get; set; }
        public string Location { get; set; }

        public SearchJobsQuery(SearchJobsRequest model)
        {
            if (model == null) return;
            Keyword = model.keyword;
            Latitude = model.lat;
            Longitude = model.lng;
            Miles = model.miles ?? 50;
            Location = model.location;
        }
    }
    public class SearchJobsQueryHandler : IRequestHandler<SearchJobsQuery, SearchJobsModel>
    {
        private readonly IApplicationContext _context;
        private IMapper _mapper;
        private IGoogleGeocodeService _geocode;

        public SearchJobsQueryHandler(
            IApplicationContext context,
            IMapper mapper,
            IGoogleGeocodeService geocode
            )
        {
            _context = context;
            _mapper = mapper;
            _geocode = geocode;
        }

        public async Task<SearchJobsModel> Handle(SearchJobsQuery request, CancellationToken cancellationToken)
        {
            Result location = null;
            if (request.Latitude != null && request.Longitude != null)
            {
                location = await _geocode.ReverseGeocode(request.Latitude.Value, request.Longitude.Value);
            }
            else if(!string.IsNullOrWhiteSpace(request.Location))
            {
                location = await _geocode.Geocode(request.Location);
            }

            if(location != null)
            {
                request.Location = location?.formatted_address;
                request.Latitude = location?.geometry.location.lat ?? 0;
                request.Longitude = location?.geometry.location.lng ?? 0;
            }

            var data = from j in _context.Jobs.AsEnumerable()
                       join c in _context.Companies.AsEnumerable() on j.CompanyID equals c.ID
                       join l in _context.JobLocations.AsEnumerable() on j.ID equals l.JobID
                       where j.IsAcceptingApplications() && j.HasKeyword(request.Keyword) && l.WithinMiles(request.Latitude, request.Longitude, request.Miles)
                       select new { j, l };

            var map = new Dictionary<int, SearchJobModel>();
            foreach (var d in data)
            {
                if (!map.ContainsKey(d.j.ID))
                    map.Add(d.j.ID, _mapper.Map<SearchJobModel>(d.j));

                map[d.j.ID].Locations.Add(_mapper.Map<JobLocationModel>(d.l));
            }
            await Task.FromResult(0);
            return new SearchJobsModel
            {
                Jobs = map.Values.ToList(),
                DisplayLocation = request.Location
            };
        }
    }
}
