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

namespace Application.Users.Commands.SetUserSkill
{
    public class SetUserSkillCommand : IRequest<bool>
    {
        public int SkillID { get; set; }
        public int UserID { get; set; }
        public SetUserSkillCommand(int skillId, int userId)
        {
            SkillID = skillId;
            UserID = userId;
        }
    }
    public class SetUserSkillCommandHandler : IRequestHandler<SetUserSkillCommand, bool>
    {
        private readonly IApplicationContext _context;
        public SetUserSkillCommandHandler(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(SetUserSkillCommand request, CancellationToken cancellationToken)
        {
            if (await _context.UserSkills.FirstOrDefaultAsync(s => s.SkillID == request.SkillID && s.UserID == request.UserID) is UserSkill userSkill)
                _context.UserSkills.Remove(userSkill);
            else
                _context.UserSkills.Add(new UserSkill
                {
                    UserID = request.UserID,
                    SkillID = request.SkillID
                });
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
