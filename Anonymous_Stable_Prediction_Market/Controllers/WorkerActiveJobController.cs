using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChoresAndFulfillment.Data;
using ChoresAndFulfillment.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChoresAndFulfillment.Controllers
{
    public class WorkerActiveJobController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _applicationDbContext;
        public WorkerActiveJobController(UserManager<User> userManager, ApplicationDbContext applicationDbContext)
        {
            this._userManager = userManager;
            this._applicationDbContext = applicationDbContext;
        }
        [HttpGet]
        [Authorize]
        public IActionResult Index(int id)
        {
            if (!IsWorker() || !IsValidJob(id))
            {
                return Redirect("/");
            }
            User user = _userManager.GetUserAsync(HttpContext.User).Result;
            Job job = _applicationDbContext.Jobs.
                Where(a => a.Id == id).Include(a => a.JobCreator).ThenInclude(a => a.User).
                First();
            ViewData["name"] = job.Name;
            ViewData["description"] = job.Description;
            ViewData["creator"] = job.JobCreator.User.UserName;
            ViewData["payment"] = job.PayUponCompletion;
            if (job.WorkerConfirmedFinished && job.EmployerConfirmedFinished)
            {
                job.JobState = JobState.Finished;
                _applicationDbContext.SaveChanges();
                return Redirect("/");
            }
            if (job.WorkerConfirmedFinished)
            {
                ViewData["markAsFinished"] =
                    "<p style=\"color:darkgreen\">" +
                    "Job marked as finished, awaiting employer confirmation..." +
                    "</p>";
            }
            else
            {
                ViewData["markAsFinished"] =
                   "<li style=\"list-style:none\">" +
                   "<a class=\"btn btn-warning\" href=\"/WorkerActiveJob/MarkAsFinished/" + job.Id + "\">" +
                    "Mark job as finished, " +
                    "and confirm that you have received payment" +
                    "</a>" +
                    "</li>" +
                    "<li style=\"list-style:none\">" +
                    "Notice: The job will be marked as finished only " +
                    "when both you and the employer " +
                    "have confirmed it as being so." +
                    "</li>";
            }
            return View();
        }
        [HttpGet]
        [Authorize]
        public IActionResult MarkAsFinished(int id)
        {
            if (!IsWorker() || !IsValidJob(id))
            {
                return Redirect("/");
            }
            Job job = _applicationDbContext.Jobs.
                Where(a => a.Id == id).Include(a => a.JobCreator).ThenInclude(a => a.User).
                First();
            job.WorkerConfirmedFinished = true;
            _applicationDbContext.SaveChanges();
            return Redirect("/WorkerActiveJob/Index/" + id);
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
        private bool IsValidJob(int id)
        {
            Job job = _applicationDbContext.Jobs.FirstOrDefault(a => a.Id == id && a.JobState == JobState.Active);
            User user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (job != null)
            {
                if (user.WorkerAccountId == job.HiredUserId)
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

    }
}