using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class Job : BaseType
    {
        /// <summary>
        /// UserID of organization member who posted job
        /// </summary>
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }

        /// <summary>
        /// CompanyID of organization that posted job.
        /// </summary>
        public int CompanyID { get; set; }
        [ForeignKey("CompanyID")]
        public Company Company { get; set; }

        /// <summary>
        /// Indicates datetime the job should be or was made public
        /// if null it is excluded from the results
        /// </summary>
        public DateTime? PostAt { get; set; }

        /// <summary>
        /// Indicates datetime the job should be or was removed
        /// if null, nothing happens
        /// </summary>
        public DateTime? ExpiresAt { get; set; }
    }
    public static class JobExtensions
    {
        /// <summary>
        /// Checks if job is active and within the availability
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsAvailable(this Job @this)
        {
            return @this.Active && 
                (@this.PostAt ?? DateTime.MaxValue) <= DateTime.Now 
                && (@this.ExpiresAt ?? DateTime.MaxValue) > DateTime.Now;
        }

        /// <summary>
        /// Check a keyword value against jobs descriptive properties (i.e. Name, Description)
        /// </summary>
        /// <param name="this"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static bool HasKeyword(this Job @this, string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return true;
            return @this.Name.ToLower().Contains(keyword.ToLower()) 
                || @this.Description.ToLower().Contains(keyword.ToLower());
        }
    }
}
