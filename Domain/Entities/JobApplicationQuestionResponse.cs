using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class JobApplicationQuestionResponse : BaseEntity
    {
        /// <summary>
        /// associated job application question
        /// </summary>
        public int JobApplicationQuestionID { get; set; }
        [ForeignKey("JobApplicationQuestionID")]
        public JobApplicationQuestion JobApplicationQuestion { get; set; }

        public string Response { get; set; }
    }
}
