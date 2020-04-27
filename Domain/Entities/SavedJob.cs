using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class SavedJob : BaseEntity
    {
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }
        public int JobID { get; set; }
        [ForeignKey("JobID")]
        public Job Job { get; set; }
    }
}
