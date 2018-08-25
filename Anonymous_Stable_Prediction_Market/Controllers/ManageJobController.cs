using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChoresAndFulfillment.Data;
using ChoresAndFulfillment.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChoresAndFulfillment.Controllers
{
    public class ManageJobController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _applicationDbContext;
        public ManageJobController(UserManager<User> userManager, ApplicationDbContext applicationDbContext)
        {
            this._userManager = userManager;
            this._applicationDbContext = applicationDbContext;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Manage(int id)
        {
            if (IsWorker() || !IsValidJob(id))
            {
                return Redirect("/");
            }
            ViewData["JobId"] = id;
            User user = _userManager.GetUserAsync(HttpContext.User).Result;
            Job job = _applicationDbContext.Jobs.
                Where(a => a.Id == id).Include(a => a.Applicants).ThenInclude(a=>a.WorkerAccount).
                ThenInclude(a=>a.User).First();
            if (!job.Applicants.Any(a=>a.ApplicationState==ApplicationState.Accepted|| 
            a.ApplicationState ==ApplicationState.Pending))
            {
                ViewData["JobApplications"] = "<h4>No one has applied for your job yet!</h4>";
                return View();
            }
            StringBuilder stringBuilder = new StringBuilder();
            if (job.Applicants.Any(a => a.ApplicationState == ApplicationState.Accepted))
            {
                WorkerAccountApplication acceptedApplication =
                    job.Applicants.First(a => a.ApplicationState == ApplicationState.Accepted);
                stringBuilder.AppendLine("<li><ul>");
                stringBuilder.AppendLine(
                    "<li>Username: " +
                    acceptedApplication.WorkerAccount.User.UserName +
                    "</li>");
                stringBuilder.AppendLine(
                    "<li>Telephone Number: " +
                    acceptedApplication.WorkerAccount.User.PhoneNumber +
                    "</li>");
                stringBuilder.AppendLine(
                    "<li>Job Application: " +
                    acceptedApplication.ApplicationMessage +
                    "</li>");
                stringBuilder.AppendLine(
                    "<li style=\"color:darkgreen\">Application Accepted</li>");
                if (job.WorkerConfirmedFinished && job.EmployerConfirmedFinished)
                {
                    Redirect("/");
                }
                if (job.EmployerConfirmedFinished)
                {
                    stringBuilder.AppendLine(
                        "<li style=\"list-style:none;color:darkgreen\">" +
                        "Job marked as finished. Awaiting worker confirmation..." +
                        "</li>");
                }
                else
                {
                    stringBuilder.AppendLine(
                        "<li style=\"list-style:none\">" +
                        "<a class=\"btn btn-warning\" href=\"/ManageJob/MarkAsFinished/"+job.Id+"\">" +
                        "Mark job as finished by the worker applicant, " +
                        "and confirm that payment has been received by the worker" +
                        "</a>" +
                        "</li>");
                    stringBuilder.AppendLine(
                        "<li style=\"list-style:none\">" +
                        "Notice: The job will be marked as finished only " +
                        "when both you and the worker applicant " +
                        "have confirmed it as being so." +
                        "</li>");
                }
                stringBuilder.AppendLine("</ul>");
            }
            foreach (var jobApplication in job.Applicants.Where(
                a =>a.ApplicationState ==ApplicationState.Pending))
            {
                stringBuilder.AppendLine("<li><ul>");
                
                stringBuilder.AppendLine(
                    "<li>Username: "+
                    jobApplication.WorkerAccount.User.UserName+
                    "</li>");
                stringBuilder.AppendLine(
                    "<li>Telephone Number: " +
                    jobApplication.WorkerAccount.User.PhoneNumber +
                    "</li>");
                stringBuilder.AppendLine(
                    "<li>Job Application: " + 
                    jobApplication.ApplicationMessage + 
                    "</li>");
                stringBuilder.AppendLine("<li>");
                stringBuilder.AppendLine(
                    "<a href=\"../AcceptApplicationFrom?jobId=" +jobApplication.JobId+"&workerAccountId="+jobApplication.WorkerAccountId + "\">Accept Application</a> "
                    );
                stringBuilder.AppendLine("</li>");

                stringBuilder.AppendLine("</ul></li>");
            }
            ViewData["JobApplications"] = stringBuilder.ToString();
            return View();
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
            Job job = _applicationDbContext.Jobs.FirstOrDefault(a=>a.Id==id&&a.JobState==JobState.Active);
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