using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Jobs.Queries.GetJobApplication
{
    public class QuestionValidationModel : IMapFrom<QuestionValidation>
    {
        public string ValidationRuleValue { get; set; }
        public string ValidationMessage { get; set; }
        public string ValidationRuleKey { get; set; }
    }
}
