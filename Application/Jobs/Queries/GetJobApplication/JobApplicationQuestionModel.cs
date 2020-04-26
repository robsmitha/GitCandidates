using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Jobs.Queries.GetJobApplication
{
    public class JobApplicationQuestionModel : IMapFrom<JobApplicationQuestion>
    {
        public JobApplicationQuestionModel()
        {
            Responses = new List<QuestionResponseModel>();
            ValidationRules = new List<QuestionValidationModel>();
        }
        public int ID { get; set; }
        public int DisplayOrder { get; set; }
        public string QuestionLabel { get; set; }
        public string QuestionPlaceholder { get; set; }
        public string QuestionDefaultValue { get; set; }
        public string QuestionResponseTypeInput { get; set; }
        public List<QuestionResponseModel> Responses { get; set; }
        public List<QuestionValidationModel> ValidationRules { get; set; }
    }
}
