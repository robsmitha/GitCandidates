using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class CompanyIndustry : BaseEntity
    {
        public int CompanyID { get; set; }
        [ForeignKey("CompanyID")]
        public Company Company { get; set; }
        public int IndustryID { get; set; }
        [ForeignKey("IndustryID")]
        public Industry Industry { get; set; }
    }
}
