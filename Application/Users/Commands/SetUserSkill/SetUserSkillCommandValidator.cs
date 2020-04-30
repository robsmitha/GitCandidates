using FluentValidation;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.SetUserSkill
{
    public class SetUserSkillCommandValidator : AbstractValidator<SetUserSkillCommand>
    {
        private readonly IApplicationContext _context;
        public SetUserSkillCommandValidator(IApplicationContext context)
        {
            _context = context;
        }
    }
}
