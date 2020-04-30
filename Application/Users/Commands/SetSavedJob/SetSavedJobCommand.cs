using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.SetSavedJob
{
    public class SetSavedJobCommand : IRequest<bool>
    {
        public int JobID { get; set; }
        public int UserID { get; set; }
        public SetSavedJobCommand(int jobId, int userId)
        {
            JobID = jobId;
            UserID = userId;
        }
    }
    public class SetSavedJobCommandHandler : IRequestHandler<SetSavedJobCommand, bool>
    {
        private readonly IApplicationContext _context;
        public SetSavedJobCommandHandler(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(SetSavedJobCommand request, CancellationToken cancellationToken)
        {
            if (await _context.SavedJobs.FirstOrDefaultAsync(j => j.JobID == request.JobID && j.UserID == request.UserID) is SavedJob savedJob)
                _context.SavedJobs.Remove(savedJob);
            else
                _context.SavedJobs.Add(new SavedJob
                {
                    JobID = request.JobID,
                    UserID = request.UserID
                });
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
