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
                       join sl in _context.SeniorityLevels.AsEnumerable() on j.SeniorityLevelID equals sl.ID
                       join jt in _context.JobTypes.AsEnumerable() on j.JobTypeID equals jt.ID
                       join c in _context.Companies.AsEnumerable() on j.CompanyID equals c.ID
                       join l in _context.JobLocations.AsEnumerable() on j.ID equals l.JobID
                       join ja in _context.JobApplications.AsEnumerable() on new { JobID = j.ID, request.UserID } equals new { ja.JobID, ja.UserID } into tmp_ja
                       from ja in tmp_ja.DefaultIfEmpty()
                       join jast in _context.JobApplicationStatusTypes.AsEnumerable() on ja?.JobApplicationStatusTypeID equals jast.ID into tmp_jast
                       from jast in tmp_jast.DefaultIfEmpty()
                       join sj in _context.SavedJobs.AsEnumerable() on new { JobID = j.ID, request.UserID } equals new { sj.JobID, sj.UserID } into tmp_sj
                       from sj in tmp_sj.DefaultIfEmpty()
                       join jb in _context.JobBenefits.AsEnumerable() on j.ID equals jb.JobID into tmp_jb
                       from jb in tmp_jb.AsEnumerable()
                       join jrs in _context.JobResponsibilities.AsEnumerable() on j.ID equals jrs.JobID into tmp_jrs
                       from jrs in tmp_jrs.DefaultIfEmpty()
                       join jrq in _context.JobRequirements.AsEnumerable() on j.ID equals jrq.JobID into tmp_jrq
                       from jrq in tmp_jrq.DefaultIfEmpty()
                       join jm in _context.JobMethods.AsEnumerable() on j.ID equals jm.JobID into tmp_jm
                       from jm in tmp_jm.DefaultIfEmpty()
                       where j.ID == request.JobID
                       select new { j, l, ja, sj, jb, jrs, jrq, jm };

            if (data == null || data.FirstOrDefault() == null) return new GetJobModel();
            var locations = new Dictionary<int, JobLocationModel>();
            var benefits = new Dictionary<int, BenefitModel>();
            var responsibilities = new Dictionary<int, ResponsibilityModel>();
            var requirements = new Dictionary<int, RequirementModel>();
            var methods = new Dictionary<int, MethodModel>();
            foreach (var d in data)
            {
                if (d.l != null && !locations.ContainsKey(d.l.ID))
                    locations.Add(d.l.ID, _mapper.Map<JobLocationModel>(d.l));

                if (d.jb != null && !benefits.ContainsKey(d.jb.ID))
                    benefits.Add(d.jb.ID, _mapper.Map<BenefitModel>(d.jb));

                if (d.jrs != null && !responsibilities.ContainsKey(d.jrs.ID))
                    responsibilities.Add(d.jrs.ID, _mapper.Map<ResponsibilityModel>(d.jrs));

                if (d.jrq != null && !requirements.ContainsKey(d.jrq.ID))
                    requirements.Add(d.jrq.ID, _mapper.Map<RequirementModel>(d.jrq));

                if (d.jm != null && !methods.ContainsKey(d.jm.ID))
                    methods.Add(d.jm.ID, _mapper.Map<MethodModel>(d.jm));
            }

            var row = data.First();
            var jobApp = data.Last().ja;
            var job = row.j;
            var savedJob = row.sj;
            var model = _mapper.Map<GetJobModel>(job);
            model.Locations = locations.Values.ToList();
            model.Benefits = benefits.Values.ToList();
            model.Responsibilities = responsibilities.Values.ToList();
            model.Requirements = requirements.Values.ToList();
            model.Methods = methods.Values.ToList();
            model.UserHasActiveApplication = jobApp != null && jobApp.IsActiveApplication();
            model.IsAcceptingApplications = job.IsAcceptingApplications();
            model.SavedJobID = savedJob != null ? savedJob.ID : 0;
            await Task.FromResult(0);
            return model;
        }
    }
}
