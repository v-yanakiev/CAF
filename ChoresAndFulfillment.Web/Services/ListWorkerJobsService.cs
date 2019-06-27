using ChoresAndFulfillment.Data;
using ChoresAndFulfillment.Models;
using ChoresAndFulfillment.Models.Enums;
using ChoresAndFulfillment.Web.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChoresAndFulfillment.Web.Services
{
    public class ListWorkerJobsService : UserAndContextRepository, IListWorkerJobsService
    {
        public ListWorkerJobsService(CAFContext applicationDbContext, 
            UserManager<User> userManager, IHttpContextAccessor httpContextAccessor) : 
            base(applicationDbContext, userManager, httpContextAccessor)
        {
        }

        public List<Job> ActiveJobs()
        {
            return
                GetWorkerAccount().Jobs.
                Where(a => a.JobState == JobState.Hiring || a.JobState == JobState.Hired).ToList();
        }

        public WorkerAccount GetWorkerAccount()
        {
            return applicationDbContext.
                WorkerAccounts.Where(a => a.Id == GetCurrentUser().WorkerAccountId).
                Include(a => a.Jobs).First();
        }

        public bool HasActiveJobs()
        {
            return GetWorkerAccount().Jobs.Any(a => a.JobState == JobState.Hiring || a.JobState == JobState.Hired);
        }
    }
}
