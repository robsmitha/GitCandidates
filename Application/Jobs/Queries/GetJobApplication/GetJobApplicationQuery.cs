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

namespace Application.Jobs.Queries.GetJobApplication
{
    public class GetJobApplicationQuery : IRequest<GetJobApplicationModel>
    {
        public int JobID { get; set; }
        public GetJobApplicationQuery(int jobId)
        {
            JobID = jobId;
        }
    }
    public class GetJobApplicationQueryHandler : IRequestHandler<GetJobApplicationQuery, GetJobApplicationModel>
    {
        private readonly IApplicationContext _context;
        private IMapper _mapper;

        public GetJobApplicationQueryHandler(
            IApplicationContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<GetJobApplicationModel> Handle(GetJobApplicationQuery request, CancellationToken cancellationToken)
        {
            var data = from j in _context.Jobs.AsEnumerable()
                       join jaq in _context.JobApplicationQuestions.AsEnumerable() on j.ID equals jaq.JobID
                       join sl in _context.SeniorityLevels.AsEnumerable() on j.SeniorityLevelID equals sl.ID
                       join jt in _context.JobTypes.AsEnumerable() on j.JobTypeID equals jt.ID
                       join c in _context.Companies.AsEnumerable() on j.CompanyID equals c.ID
                       join u in _context.Users.AsEnumerable() on j.UserID equals u.ID
                       join l in _context.JobLocations.AsEnumerable() on j.ID equals l.JobID
                       join q in _context.Questions.AsEnumerable() on jaq.QuestionID equals q.ID
                       join qrt in _context.ResponseTypes.AsEnumerable() on q.ResponseTypeID equals qrt.ID
                       join qr in _context.QuestionResponses.AsEnumerable() on jaq.QuestionID equals qr.QuestionID into tmp_qr
                       from qr in tmp_qr.DefaultIfEmpty()
                       join qv in _context.QuestionValidations.AsEnumerable() on jaq.QuestionID equals qv.QuestionID into tmp_qv
                       from qv in tmp_qv.DefaultIfEmpty()
                       join vr in _context.ValidationRules.AsEnumerable() on qv.ValidationRuleID equals vr.ID into tmp_vr
                       from vr in tmp_vr.DefaultIfEmpty()
                       where jaq.JobID == request.JobID && j.IsAcceptingApplications()
                       select new {
                           jaq, 
                           qr,
                           qv,
                           j,
                           l
                       };
            var first = data?.FirstOrDefault();
            if (first == null) return new GetJobApplicationModel();
            var dict_jaq = new Dictionary<int, JobApplicationQuestionModel>();
            var hs_qr = new HashSet<int>();
            var hs_qv = new HashSet<int>();
            var dict_l = new Dictionary<int, JobLocationModel>();
            foreach (var row in data)
            {
                if (!dict_jaq.TryGetValue(row.jaq.ID, out var jaq))
                {
                    jaq = _mapper.Map<JobApplicationQuestionModel>(row.jaq);
                    dict_jaq.Add(row.jaq.ID, jaq);
                }

                if (row.qr != null && !hs_qr.Contains(row.qr.ID))
                {
                    hs_qr.Add(row.qr.ID);
                    jaq.Responses.Add(_mapper.Map<QuestionResponseModel>(row.qr));
                    dict_jaq[row.jaq.ID] = jaq;
                }

                if (row.qv != null && !hs_qv.Contains(row.qv.ID))
                {
                    hs_qv.Add(row.qv.ID);
                    jaq.ValidationRules.Add(_mapper.Map<QuestionValidationModel>(row.qv));
                    dict_jaq[row.jaq.ID] = jaq;
                }

                if (row.l != null && !dict_l.ContainsKey(row.l.ID))
                {
                    dict_l.Add(row.l.ID, _mapper.Map<JobLocationModel>(row.l));
                }
            }
            var job = _mapper.Map<JobModel>(first.j);
            job.Locations = dict_l.Values.ToList();
            return await Task.FromResult(new GetJobApplicationModel
            {
                Job = job,
                Questions = dict_jaq.Values.ToList()
            }); 
        }
    }
}
