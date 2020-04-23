using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
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
            var entity = await _context.Jobs.FindAsync(request.JobID);
            if (entity == null) return new GetJobModel();
            return _mapper.Map<GetJobModel>(entity);
        }
    }
}
