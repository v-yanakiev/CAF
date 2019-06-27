using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChoresAndFulfillment.Data;
using ChoresAndFulfillment.Models;
using ChoresAndFulfillment.Web.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ChoresAndFulfillment.Web.Services
{
    public class ApplyForJobService : UserAndContextRepository, IApplyForJobService
    {
        public ApplyForJobService(CAFContext applicationDbContext, UserManager<User> userManager, 
            IHttpContextAccessor httpContextAccessor) : 
            base(applicationDbContext, userManager, httpContextAccessor)
        {
        }

        public bool AlreadyAppliedForJob(int jobId)
        {
            Job job = GetJob(jobId);
            return job.Applicants.Any(a => a.WorkerAccountId == GetCurrentUser().WorkerAccountId); 
        }
        public void SignUpWorkerForJob(int? workerAccountId, int jobId,string message)
        {
            WorkerAccountApplication workerAccountApplication = new WorkerAccountApplication()
            {
                ApplicationMessage = message,
                JobId = jobId,
                WorkerAccountId = (int)workerAccountId
            };
            applicationDbContext.WorkerAccountApplications.Add(workerAccountApplication);
            applicationDbContext.SaveChanges();
        }
    }

}
