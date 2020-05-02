using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Data
{
    public class ApplicationContext : DbContext, IApplicationContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>();
            modelBuilder.Entity<Job>();
            modelBuilder.Entity<JobApplication>();
            modelBuilder.Entity<JobApplicationQuestion>();
            modelBuilder.Entity<JobApplicationQuestionResponse>();
            modelBuilder.Entity<JobApplicationStatusType>();
            modelBuilder.Entity<JobLocation>();
            modelBuilder.Entity<JobSkill>();
            modelBuilder.Entity<Question>();
            modelBuilder.Entity<QuestionResponse>();
            modelBuilder.Entity<QuestionValidation>();
            modelBuilder.Entity<ResponseType>();
            modelBuilder.Entity<SavedJob>();
            modelBuilder.Entity<Skill>();
            modelBuilder.Entity<User>();
            modelBuilder.Entity<UserSkill>();
            modelBuilder.Entity<UserStatusType>();
            modelBuilder.Entity<ValidationRule>();
            modelBuilder.Entity<CompanyHighlight>();
            modelBuilder.Entity<CompanyImpact>();
            modelBuilder.Entity<CompanyIndustry>();
            modelBuilder.Entity<Industry>();
            modelBuilder.Entity<JobBenefit>();
            modelBuilder.Entity<JobMethod>();
            modelBuilder.Entity<JobRequirement>();
            modelBuilder.Entity<JobResponsibility>();
            modelBuilder.Entity<JobType>();
            modelBuilder.Entity<SeniorityLevel>();

            modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())
                .ToList()
                .ForEach(r => r.DeleteBehavior = DeleteBehavior.Restrict);
        }
        public bool EnsureCreated()
        {
            return Database.EnsureCreated();
        }
    }
}
