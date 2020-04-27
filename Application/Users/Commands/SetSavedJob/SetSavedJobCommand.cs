using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.SetSavedJob
{
    public class SetSavedJobCommand : IRequest<bool>
    {
        public int SavedJobID { get; set; }
        public int JobID { get; set; }
        public int UserID { get; set; }
        public SetSavedJobCommand(int jobId, int userId, int savedJobId )
        {
            JobID = jobId;
            UserID = userId;
            SavedJobID = savedJobId;
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
            SavedJob savedJob;
            if(request.SavedJobID > 0)
            {
                savedJob = await _context.SavedJobs.FindAsync(request.SavedJobID);
                _context.SavedJobs.Remove(savedJob);
            }
            else
            {
                savedJob = new SavedJob
                {
                    JobID = request.JobID,
                    UserID = request.UserID
                };
                _context.SavedJobs.Add(savedJob);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

    }
}
