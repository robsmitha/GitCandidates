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
        /// response type for question (i.e. text, radio, etc.)
        /// </summary>
        public int ResponseTypeID { get; set; }
        [ForeignKey("ResponseTypeID")]
        public ResponseType ResponseType { get; set; }

        public string Label { get; set; }
        public string Placeholder { get; set; }
        public string DefaultValue { get; set; }
    }
}
