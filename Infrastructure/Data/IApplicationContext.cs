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
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<JobApplicationQuestion> JobApplicationQuestions { get; set; }
        public DbSet<JobApplicationQuestionResponse> JobApplicationQuestionResponses { get; set; }
        public DbSet<JobApplicationStatusType> JobApplicationStatusTypes { get; set; }
        public DbSet<JobLocation> JobLocations { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionResponse> QuestionResponses { get; set; }
        public DbSet<QuestionValidation> QuestionValidations { get; set; }
        public DbSet<ResponseType> ResponseTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserStatusType> UserStatusTypes { get; set; }
        public DbSet<ValidationRule> ValidationRules { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        int SaveChanges();
        public bool EnsureCreated();
    }
}
