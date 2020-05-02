using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Jobs.Queries.GetJobApplication
{
    public class GetJobApplicationModel 
    {
        public JobModel Job { get; set; }
        public List<JobApplicationQuestionModel> Questions { get; set; }
    }
}
