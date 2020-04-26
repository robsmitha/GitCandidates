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


        /// <summary>
        /// Application status
        /// </summary>
        public int JobApplicationStatusTypeID { get; set; }
        [ForeignKey("JobApplicationStatusTypeID")]
        public JobApplicationStatusType JobApplicationStatusType { get; set; }
    }
    public static class JobApplicationExtensions
    {
        /// <summary>
        /// The application is considered active 
        /// ACTIVE applications are set to Submitted, Under Review, Scheduling Interview
        /// INACTIVE applications are set to Withdrawn, No Longer Under Consideration 
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsActiveApplication(this JobApplication @this)
        {
            if(@this?.JobApplicationStatusType == null)
            {
                //TODO: throw null required property in extenstion exception
            }

            return @this?.JobApplicationStatusType != null 
                && @this.JobApplicationStatusType.IsActiveApplication;
        }
    }
}
