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

namespace Application.Users.Commands.WithdrawApplication
{
    public class WithdrawApplicationCommandValidator : AbstractValidator<WithdrawApplicationCommand>
    {
        private readonly IApplicationContext _context;
        public WithdrawApplicationCommandValidator(IApplicationContext context)
        {
            _context = context;

            RuleFor(l => l.JobApplicationID).MustAsync(BeActiveApplication)
                    .WithMessage("The application must be considered an active application type.");

            RuleFor(l => l.UserID).MustAsync(BelongToUser)
                    .WithMessage("The application must be withdrawn by the user that submitted it.");

        }

        public async Task<bool> BelongToUser(WithdrawApplicationCommand args, int userId,
            CancellationToken cancellationToken)
        {
            return await _context.JobApplications
                .FirstOrDefaultAsync(a => a.ID == args.JobApplicationID && a.UserID == userId) != null;
        }

        public async Task<bool> BeActiveApplication(WithdrawApplicationCommand args, int jobApplicationId,
            CancellationToken cancellationToken)
        {
            var data = from ja in _context.JobApplications.AsEnumerable()
                      join jast in _context.JobApplicationStatusTypes.AsEnumerable() on ja.JobApplicationStatusTypeID equals jast.ID
                      where ja.ID == jobApplicationId && ja.IsActiveApplication()
                      select ja;
            if (data == null) return false;
            await Task.FromResult(0);
            return data.FirstOrDefault() != null;
        }

    }
}
