﻿using Domain.Entities;
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
        }
    }
}
