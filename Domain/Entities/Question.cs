using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class Question : BaseEntity
    {
        /// <summary>
        /// Company that questions belongs to
        /// </summary>
        public int CompanyID { get; set; }
        [ForeignKey("CompanyID")]
        public Company Company { get; set; }

        /// <summary>
        /// Question type for question
        /// </summary>
        public int QuestionTypeID { get; set; }
        [ForeignKey("QuestionTypeID")]
        public QuestionType QuestionType { get; set; }
    }
}
