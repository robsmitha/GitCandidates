using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class JobApplicationQuestion : BaseEntity
    {
        /// <summary>
        /// Parent JobID for question
        /// </summary>
        public int JobID { get; set; }
        [ForeignKey("JobID")]
        public Job Job { get; set; }

        /// <summary>
        /// QuestionID for question
        /// </summary>
        public int QuestionID { get; set; }
        [ForeignKey("QuestionID")]
        public Question Question { get; set; }

    }
}
