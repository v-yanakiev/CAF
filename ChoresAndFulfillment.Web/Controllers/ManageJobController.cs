using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChoresAndFulfillment.Data;
using ChoresAndFulfillment.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


using ChoresAndFulfillment.Models;
using ChoresAndFulfillment.Web.Data.ViewModels;

namespace ChoresAndFulfillment.Controllers
{
    public class ManageJobController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly CAFContext _applicationDbContext;
        public ManageJobController(UserManager<User> userManager, CAFContext applicationDbContext)
        {
            this._userManager = userManager;
            this._applicationDbContext = applicationDbContext;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Manage(int id)
        {
            ManageJobViewModel model = new ManageJobViewModel();
            if (IsWorker() || !IsValidJob(id))
            {
                return Redirect("/");
            }
            User user = _userManager.GetUserAsync(HttpContext.User).Result;
            Job job = _applicationDbContext.Jobs.
                Where(a => a.Id == id).Include(a => a.Applicants).ThenInclude(a=>a.WorkerAccount).
                ThenInclude(a=>a.User).First();
            if (job.WorkerConfirmedFinished && job.EmployerConfirmedFinished)
            {
                job.JobState = JobState.Finished;
                _applicationDbContext.SaveChanges();
                return Redirect("/");
            }
            model.Job = job;
            model.AnyApplications = job.Applicants.Any(a => a.ApplicationState == ApplicationState.Accepted ||
                a.ApplicationState == ApplicationState.Pending);
            if (!model.AnyApplications)
            {
                return View(model);
            }
            model.AcceptedAnyone = job.Applicants.Any(a => a.ApplicationState == ApplicationState.Accepted);
            if (model.AcceptedAnyone)
            {
                model.AcceptedApplication = model.Job.Applicants.First(a => a.ApplicationState == ApplicationState.Accepted); ;
                return View(model);
            }
            else 
            {
                model.PendingApplications = model.Job.Applicants.Where(
                    a => a.ApplicationState == ApplicationState.Pending);
                return View(model);
            }
        }
        [Authorize]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (IsWorker()||!IsValidJob(id))
            {
                return Redirect("/");
            }
            Job job= _applicationDbContext.Jobs.First(a => a.Id == id);
            job.JobState = JobState.Deactivated;
            var toRenderInactive =
                _applicationDbContext.WorkerAccountApplications.Where(a => a.JobId == job.Id);
            foreach (var application in toRenderInactive)
            {
                application.ApplicationState = ApplicationState.Deleted;
            }
            _applicationDbContext.SaveChanges();
            return Redirect("/YourActiveJobs/Index");
        }
        [Authorize]
        [HttpGet]
        public IActionResult AcceptApplicationFrom(int jobId,int workerAccountId)
        {
            if (IsWorker() || !IsValidJob(jobId)||
                !IsValidWorkerAccountApplication(jobId,workerAccountId))
            {
                return Redirect("/");
            }
            Job job =_applicationDbContext.
                     Jobs.
                     Where(a => a.Id==jobId).
                     Include(a=>a.Applicants).
                     First();
            job.HiredUserId = workerAccountId;
            job.JobState = JobState.Hired;
            foreach (var application in job.Applicants)
            {
                if (application.WorkerAccountId == workerAccountId)
                {
                    application.ApplicationState = ApplicationState.Accepted;
                }
                else
                {
                    application.ApplicationState = ApplicationState.Rejected;
                }
            }
            _applicationDbContext.SaveChanges();
            return Redirect("Manage/" + jobId);
        }
        [Authorize]
        [HttpGet]
        public IActionResult MarkAsFinished(int id)
        {
            if (IsWorker() || !IsValidJob(id))
            {
                return Redirect("/");
            }
            Job job = _applicationDbContext.Jobs.FirstOrDefault(a => a.Id == id);
            job.EmployerConfirmedFinished = true;
            _applicationDbContext.SaveChanges();
            return Redirect("/ManageJob/Manage/"+id);
        }
        private bool IsValidWorkerAccountApplication(int jobId, int workerAccountId)
        {
            WorkerAccountApplication workerAccount =
                _applicationDbContext.
                WorkerAccountApplications.
                First(a => a.WorkerAccountId == workerAccountId && a.JobId == jobId);
            if (workerAccount == null)
            {
                return false;
            }
            User user = _userManager.GetUserAsync(HttpContext.User).Result;
            Job job =
                _applicationDbContext.
                Jobs.
                Where(a => a.JobCreatorId == user.EmployerAccountId).First();
            if (job.JobCreatorId == user.EmployerAccountId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool IsValidJob( int id)
        {
            Job job = _applicationDbContext.Jobs.FirstOrDefault(a=>a.Id==id&&(a.JobState==JobState.Hiring||a.JobState==JobState.Hired));
            User user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (job != null)
            {
                if (user.EmployerAccountId == job.JobCreatorId)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private bool IsWorker()
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            if (currentUser.AccountType == "EmployerAccount")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}