using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.WithdrawApplication
{
    public class WithdrawApplicationCommand : IRequest<bool>
    {
        public int UserID { get; set; }
        public int JobApplicationID { get; set; }
        public WithdrawApplicationCommand(int id, int userId)
        {
            JobApplicationID = id;
            UserID = userId;
        }
    }
    public class WithdrawApplicationCommandHandler : IRequestHandler<WithdrawApplicationCommand, bool>
    {
        private readonly IApplicationContext _context;
        public WithdrawApplicationCommandHandler(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(WithdrawApplicationCommand request, CancellationToken cancellationToken)
        {
            var jobApplication = await _context.JobApplications
                .FindAsync(request.JobApplicationID);
            var withdrawn = await _context.JobApplicationStatusTypes
                .FirstOrDefaultAsync(s => s.Name.ToLower() == "withdrawn");
            if (withdrawn == null) return false;
            jobApplication.JobApplicationStatusTypeID = withdrawn.ID;
            _context.JobApplications.Update(jobApplication);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
