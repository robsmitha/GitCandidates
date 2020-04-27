using AutoMapper;
using Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Jobs.Queries.GetJobSkills
{
    public class GetJobSkillsQuery : IRequest<List<GetJobSkillModel>>
    {
        public int JobID { get; set; }
        public int UserID { get; set; }
        public GetJobSkillsQuery(int jobId, int userId)
        {
            JobID = jobId;
            UserID = userId;
        }
    }
    public class GetJobSkillsQueryHandler : IRequestHandler<GetJobSkillsQuery, List<GetJobSkillModel>>
    {
        private readonly IApplicationContext _context;
        private readonly IMapper _mapper;
        public GetJobSkillsQueryHandler(IApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<GetJobSkillModel>> Handle(GetJobSkillsQuery request, CancellationToken cancellationToken)
        {
            var data = from js in _context.JobSkills.AsEnumerable()
                       join s in _context.Skills.AsEnumerable() on js.SkillID equals s.ID
                       join us in _context.UserSkills.AsEnumerable() on new { SkillID = s.ID, request.UserID } equals new { us.SkillID, us.UserID } into tmp_us
                       from us in tmp_us.DefaultIfEmpty()
                       where js.JobID == request.JobID
                       select new { js, us };

            if (data == null || data.FirstOrDefault() == null) return new List<GetJobSkillModel>();

            var model = new List<GetJobSkillModel>();
            foreach(var d in data)
            {
                var skill = _mapper.Map<GetJobSkillModel>(d.js);
                skill.UserHasSkill = d.us != null;
                model.Add(skill);
            }
            await Task.FromResult(0);
            return model;
        }
    }
}
