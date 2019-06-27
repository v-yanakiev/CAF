using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ChoresAndFulfillment.Models;
namespace ChoresAndFulfillment.Data
{
    public class CAFContext : IdentityDbContext<User>
    {

        public CAFContext(DbContextOptions<CAFContext> options)
            : base(options)
        {
        }
        public DbSet<WorkerAccount> WorkerAccounts { get; set; }
        public DbSet<EmployerAccount> EmployerAccounts { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<WorkerAccountApplication> WorkerAccountApplications { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Job>().
                HasOne(workTask => workTask.JobCreator).
                WithMany(taskCreator => taskCreator.CreatedJobs);
            builder.Entity<WorkerAccount>().
                HasMany(workerAccount => workerAccount.Jobs).
                WithOne(job => job.HiredUser);
            builder.Entity<User>().
                HasOne(user => user.EmployerAccount).
                WithOne(employerAccount => employerAccount.User).
                HasForeignKey<User>(user => user.EmployerAccountId);
            builder.Entity<User>().
                HasOne(user => user.WorkerAccount).
                WithOne(workerAccount => workerAccount.User).
                HasForeignKey<User>(user => user.WorkerAccountId);
            builder.Entity<Message>().
                HasOne(message => message.MessageSender).
                WithMany(poster => poster.MessagesSent).
                HasForeignKey(message => message.MessageSenderId);
            builder.Entity<Message>().
                HasOne(message => message.MessageReceiver).
                WithMany(receiver => receiver.MessagesReceived).
                HasForeignKey(message => message.MessageReceiverId);
            builder.Entity<WorkerAccountApplication>().
                HasKey(WAP => new { WAP.WorkerAccountId, WAP.JobId });
            builder.Entity<Job>().
                HasMany(workTask => workTask.Applicants).
                WithOne(applicant => applicant.Job);
            builder.Entity<WorkerAccount>().
                HasMany(workerAccount => workerAccount.ActiveApplications).
                WithOne(activeApplication => activeApplication.WorkerAccount);
            
        }
    }
}
