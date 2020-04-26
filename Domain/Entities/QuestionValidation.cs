using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class QuestionValidation : BaseEntity
    {
        /// <summary>
        /// Question that validation is associated to
        /// </summary>
        public int QuestionID { get; set; }
        [ForeignKey("QuestionID")]
        public Question Question { get; set; }

        /// <summary>
        /// Validation Rule the QuestionValidationRule is associated to
        /// </summary>
        public int ValidationRuleID { get; set; }
        [ForeignKey("ValidationRuleID")]
        public ValidationRule ValidationRule { get; set; }

        public string ValidationRuleValue { get; set; }
        public string ValidationMessage { get; set; }
    }
}
