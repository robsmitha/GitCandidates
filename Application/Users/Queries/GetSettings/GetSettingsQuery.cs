using AutoMapper;
using Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetSettings
{
    public class GetSettingsQuery : IRequest<GetSettingsModel>
    {
        public int UserID { get; set; }
        public GetSettingsQuery(int userId)
        {
            UserID = userId;
        }
    }
    public class GetSettingsQueryHandler : IRequestHandler<GetSettingsQuery, GetSettingsModel>
    {
        private readonly IApplicationContext _context;
        private readonly IMapper _mapper;
        public GetSettingsQueryHandler(IApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetSettingsModel> Handle(GetSettingsQuery request, CancellationToken cancellationToken)
        {
            var data = from s in _context.Skills.AsEnumerable()
                       join us in _context.UserSkills.AsEnumerable() on new { SkillID = s.ID, request.UserID } equals new { us.SkillID, us.UserID } into tmp_us
                       from us in tmp_us.DefaultIfEmpty()
                       select new { s, us };
            if (data == null || data.FirstOrDefault() == null) return new GetSettingsModel();
            var model = new GetSettingsModel();
            foreach(var row in data)
            {
                model.Skills.Add(new UserSkillModel(
                    skill: _mapper.Map<SkillModel>(row.s),
                    userSkill: row.us));
            }
            await Task.FromResult(0);
            return model;
        }
    }
}
