using ChoresAndFulfillment.Data;
using ChoresAndFulfillment.Models;
using ChoresAndFulfillment.Models.Enums;
using ChoresAndFulfillment.Web.Data.ViewModels;
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
    public class ListAllJobsService : UserAndContextRepository, IListAllJobsService
    {
        public const int JobsPerPage = 5 ;
        public ListAllJobsService
            (CAFContext applicationDbContext, UserManager<User> userManager, 
            IHttpContextAccessor httpContextAccessor) : 
            base(applicationDbContext, userManager, httpContextAccessor)
        {
        }
        public bool PageIsOutOfBounds(int pageNumber)
        {
            if (pageNumber <= 0)
            {
                return true;
            }
            if (PageIsLast(pageNumber - 1)) //Due to the mechanism of the PageIsLast method, it doesn't matter whether 'pageNumber - 1' yields the number of an actual existing page.
            {
                return true;
            }
            return false;
        }
        public bool PageIsLast(int pageNumber)
        {
            int jobCount = applicationDbContext.Jobs.Where(job => job.JobState == JobState.Hiring).Count();
            Console.WriteLine(jobCount);
            return ( jobCount<= (pageNumber * JobsPerPage));
        }
        public List<ActiveJobViewModel> ViewAllActiveJobs(int pageNumber)
        {
            return applicationDbContext.Jobs.Include(job => job.JobCreator).
                    ThenInclude(jobCreator => jobCreator.User).
                    ThenInclude(user => user.RatingsReceived).Where(job => job.JobState == JobState.Hiring).
                    Select(a=>new ActiveJobViewModel()
                    {
                      Id=a.Id,
                      Name=a.Name,
                      Description=a.Description,
                      JobCreatorName=a.JobCreator.User.UserName,
                      JobCreatorRating=a.JobCreator.User.Rating,
                      PayUponCompletion=a.PayUponCompletion
                    }).Skip((pageNumber-1)*JobsPerPage).Take(JobsPerPage).ToList();
        }

        public bool AnyActiveJobs()
        {
            if (applicationDbContext.Jobs.Any(a => a.JobState == JobState.Hiring))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool WorkerAlreadyApplied(int jobId)
        {
            WorkerAccount workerAccount = applicationDbContext.
                WorkerAccounts.First
                (a=>a.Id==GetCurrentUser().WorkerAccountId);
            Job job = applicationDbContext.Jobs.Where(a => a.Id == jobId).
                Include(a => a.Applicants).First();
            if (workerAccount.ActiveApplications.Any(a => a.JobId == job.Id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
