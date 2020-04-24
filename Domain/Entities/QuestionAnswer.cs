using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class QuestionAnswer : BaseEntity
    {
        /// <summary>
        /// QuestionID for question
        /// </summary>
        public int QuestionID { get; set; }
        [ForeignKey("QuestionID")]
        public Question Question { get; set; }
    }
}
