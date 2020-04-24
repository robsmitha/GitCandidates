using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class JobApplication : BaseEntity
    {
        /// <summary>
        /// UserID of applicant
        /// </summary>
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }

        /// <summary>
        /// Job the application is for
        /// </summary>
        public int JobID { get; set; }
        [ForeignKey("JobID")]
        public Job Job { get; set; }
    }
}
