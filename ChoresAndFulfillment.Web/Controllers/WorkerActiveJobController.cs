using System;
using System.Collections.Generic;
using System.Linq;
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
    public class WorkerActiveJobController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly CAFContext _applicationDbContext;
        public WorkerActiveJobController(UserManager<User> userManager, CAFContext applicationDbContext)
        {
            this._userManager = userManager;
            this._applicationDbContext = applicationDbContext;
        }
        [HttpGet]
        [Authorize]
        public IActionResult Index(int id)
        {
            WorkerActiveJobViewModel model = new WorkerActiveJobViewModel();
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
                return Redirect("/ListAllJobs");
            }
            model.JobId = job.Id;
            model.WorkerConfirmedFinished = job.WorkerConfirmedFinished;
            model.JobName = job.Name;
            model.JobDescription = job.Description;
            model.PayUponCompletion = job.PayUponCompletion.ToString();
            return View(model);
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
            Job job = _applicationDbContext.Jobs.FirstOrDefault(a => a.Id == id && (a.JobState == JobState.Hiring||a.JobState==JobState.Hired));
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