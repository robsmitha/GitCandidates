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
        public DbSet<User> Users { get; set; }
        public DbSet<UserStatusType> UserStatusTypes { get; set; }
        public DbSet<Job> Jobs { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        int SaveChanges();
        public bool EnsureCreated();
    }
}
