using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class JobResponsibility : BaseType
    {
        public int JobID { get; set; }
        [ForeignKey("JobID")]
        public Job Job { get; set; }
    }
}
