﻿using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Jobs.Queries.GetJob
{
    public class RequirementModel : IMapFrom<JobRequirement>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}