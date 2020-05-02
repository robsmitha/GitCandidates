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
        /// Job post full content
        /// </summary>
        public string PostHTML { get; set; }

        /// <summary>
        /// Indicates datetime the job should be or was removed
        /// if null, nothing happens
        /// </summary>
        public DateTime? ExpiresAt { get; set; }

        public string TeamSize { get; set; }
        public string Travel { get; set; }

        public bool? AllowRemote { get; set; }
        public int? MinSalary { get; set; }
        public int? MaxSalary { get; set; }

        /// <summary>
        /// Seniority Level for job (i.e. Intern, Junior, Middle, Senior, Lead)
        /// </summary>
        public int SeniorityLevelID { get; set; }
        [ForeignKey("SeniorityLevelID")]
        public SeniorityLevel SeniorityLevel { get; set; }

        /// <summary>
        /// Occupation type for job (i.e. Full-time, Part-time, Internship)
        /// </summary>
        public int JobTypeID { get; set; }
        [ForeignKey("JobTypeID")]
        public JobType JobType { get; set; }
    }
    public static class JobExtensions
    {
        /// <summary>
        /// Checks if job is active and within the availability
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsAcceptingApplications(this Job @this)
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

        /// <summary>
        /// Gets elapsed time string in english
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string ToElapsedTime(this DateTime @this)
        {
            var elapsedTime = DateTime.Now.Subtract(@this);
            if ((int)elapsedTime.TotalDays > 13) return $"{(int)elapsedTime.TotalDays / 7} weeks ago";
            if ((int)elapsedTime.TotalDays == 7) return $"a week ago";
            if ((int)elapsedTime.TotalDays > 1) return $"{(int)elapsedTime.TotalDays} days ago";
            if ((int)elapsedTime.TotalDays == 1) return $"a day ago";
            if ((int)elapsedTime.TotalHours > 1) return $"{(int)elapsedTime.TotalHours} hours ago";
            if ((int)elapsedTime.TotalHours == 1) return $"one hour ago";
            return "New";
        }
    }
}
