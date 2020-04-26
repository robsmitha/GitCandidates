using System.Collections.Generic;

namespace Application.Jobs.Commands.CreateJobApplication
{

    public class CreateJobApplicationModel
    {
        public int JobID { get; set; }
        public List<JobApplicationQuestionResponseModel> Responses { get; set; }
    }
}