using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public interface IApplicationContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyHighlight> CompanyHighlights { get; set; }
        public DbSet<CompanyIndustry> CompanyIndustries { get; set; }
        public DbSet<CompanyImpact> CompanyImpacts { get; set; }
        public DbSet<Industry> Industries { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobBenefit> JobBenefits { get; set; }
        public DbSet<JobMethod> JobMethods { get; set; }
        public DbSet<JobRequirement> JobRequirements { get; set; }
        public DbSet<JobResponsibility> JobResponsibilities { get; set; }
        public DbSet<JobType> JobTypes { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<JobApplicationQuestion> JobApplicationQuestions { get; set; }
        public DbSet<JobApplicationQuestionResponse> JobApplicationQuestionResponses { get; set; }
        public DbSet<JobApplicationStatusType> JobApplicationStatusTypes { get; set; }
        public DbSet<JobLocation> JobLocations { get; set; }
        public DbSet<JobSkill> JobSkills { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionResponse> QuestionResponses { get; set; }
        public DbSet<QuestionValidation> QuestionValidations { get; set; }
        public DbSet<ResponseType> ResponseTypes { get; set; }
        public DbSet<SavedJob> SavedJobs { get; set; }
        public DbSet<SeniorityLevel> SeniorityLevels { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserSkill> UserSkills { get; set; }
        public DbSet<UserStatusType> UserStatusTypes { get; set; }
        public DbSet<ValidationRule> ValidationRules { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        int SaveChanges();
        public bool EnsureCreated();
    }
}
