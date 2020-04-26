using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Jobs.Queries.GetJobApplication
{
    public class QuestionResponseModel : IMapFrom<QuestionResponse>
    {
        public int ID { get; set; }
        public string Answer { get; set; }
        public int DisplayOrder { get; set; }

    }
}
