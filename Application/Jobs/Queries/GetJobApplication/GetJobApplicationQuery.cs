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
            var data = from jaq in _context.JobApplicationQuestions.AsEnumerable()
                       join q in _context.Questions.AsEnumerable() on jaq.QuestionID equals q.ID
                       join qrt in _context.ResponseTypes.AsEnumerable() on q.ResponseTypeID equals qrt.ID
                       join j in _context.Jobs.AsEnumerable() on jaq.JobID equals j.ID
                       join c in _context.Companies.AsEnumerable() on j.CompanyID equals c.ID
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
                           qv
                       };

            if (data?.FirstOrDefault() == null) return new GetJobApplicationModel();

            var map = new Dictionary<int, JobApplicationQuestionModel>();
            foreach (var row in data)
            {
                if (!map.TryGetValue(row.jaq.ID, out var jaq))
                {
                    jaq = _mapper.Map<JobApplicationQuestionModel>(row.jaq);
                    map.Add(row.jaq.ID, jaq);
                }

                if (row.qr != null)
                {
                    jaq.Responses.Add(_mapper.Map<QuestionResponseModel>(row.qr));
                    map[row.jaq.ID] = jaq;
                }

                if (row.qv != null)
                {
                    jaq.ValidationRules.Add(_mapper.Map<QuestionValidationModel>(row.qv));
                    map[row.jaq.ID] = jaq;
                }
            }
            await Task.FromResult(0);
            var job = data.First().jaq.Job;
            return new GetJobApplicationModel
            {
                JobName = job.Name,
                CompanyGitHubLogin = job.Company.GitHubLogin,
                Questions = map.Values.ToList()
            };
        }
    }
}
