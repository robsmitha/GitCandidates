using Domain.Entities;
using FluentValidation;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Jobs.Commands.CreateJobApplication
{
    public class CreateJobApplicationCommandValidator 
        : AbstractValidator<CreateJobApplicationCommand>
    {
        private readonly IApplicationContext _context;
        public CreateJobApplicationCommandValidator(IApplicationContext context)
        {
            _context = context;

            RuleFor(a => a.JobID).MustAsync(JobIsAcceptingApplications)
                    .WithMessage("The job is not accepting applications at this time.");

            RuleFor(a => a.JobID).MustAsync(UserHasNoActiveApplication)
                    .WithMessage("You have already applied for this job.");

            RuleFor(a => a.UserID).MustAsync(UserCanApply)
                    .WithMessage("You cannot apply at this time.");
        }
        /// <summary>
        /// Checks the jobs is currently accepting applications
        /// </summary>
        /// <param name="args"></param>
        /// <param name="jobId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> JobIsAcceptingApplications(CreateJobApplicationCommand args, int jobId,
            CancellationToken cancellationToken)
        {
            var job = await _context.Jobs.FindAsync(jobId);
            return job.IsAcceptingApplications();
        }

        /// <summary>
        /// Checks if the user has applied for the job before
        /// </summary>
        /// <param name="args"></param>
        /// <param name="jobId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> UserHasNoActiveApplication(CreateJobApplicationCommand args, int jobId,
            CancellationToken cancellationToken)
        {
            var data = from ja in _context.JobApplications.AsEnumerable()
                       join j in _context.Jobs on ja.JobID equals j.ID
                       join jast in _context.JobApplicationStatusTypes on ja.JobApplicationStatusTypeID equals jast.ID
                       where ja.JobID == jobId && ja.UserID == args.UserID && ja.IsActiveApplication()
                       select ja;

            return await Task.FromResult(data == null || data.FirstOrDefault() == null);
        }

        /// <summary>
        /// Check if the user is eligible to apply (i.e. they have confirmed their email)
        /// </summary>
        /// <param name="args"></param>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> UserCanApply(CreateJobApplicationCommand args, int userId,
            CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(s => s.UserStatusType)
                .FirstOrDefaultAsync(u => u.ID == userId);
            if (user == null) return false;
            return user.CanApply();
        }
    }
}
