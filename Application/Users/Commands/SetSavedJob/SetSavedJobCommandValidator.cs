using Domain.Entities;
using FluentValidation;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.SetSavedJob
{
    public class SetSavedJobCommandValidator : AbstractValidator<SetSavedJobCommand>
    {
        private readonly IApplicationContext _context;
        public SetSavedJobCommandValidator(IApplicationContext context)
        {
            _context = context;
            RuleFor(l => l.UserID).MustAsync(BelongToUser)
                    .WithMessage("The saved job can only be set by the user that it belongs to.");
        }

        public async Task<bool> BelongToUser(SetSavedJobCommand args, int userId,
            CancellationToken cancellationToken)
        {
            return args.SavedJobID == null || args.SavedJobID == 0
                    ? true
                    : await _context.SavedJobs
                    .FirstOrDefaultAsync(a => a.ID == args.SavedJobID && a.UserID == userId) != null;
        }
    }
}
