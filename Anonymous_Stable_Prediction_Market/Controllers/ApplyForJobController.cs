using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChoresAndFulfillment.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


using ChoresAndFulfillment.Models;
namespace ChoresAndFulfillment.Controllers
{
    public class ApplyForJobController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly CAFContext _applicationDbContext;
        public ApplyForJobController(UserManager<User> userManager, CAFContext applicationDbContext)
        {
            this._userManager = userManager;
            this._applicationDbContext = applicationDbContext;
        }
        [Authorize]
        [HttpGet]
        public IActionResult Apply(int id)
        {
            ViewData["error"] = "";
            Job job = _applicationDbContext.Jobs.Where(a=>a.Id==id).Include(a=>a.Applicants).First();
            if (!IsWorker()||job==null)
            {
                return Redirect("/");
            }
            ViewData["JobName"] = job.Name;
            ViewData["NumberOfApplicants"] = job.Applicants.Count;
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult Apply(int id, string JobApplicationMessage)
        {
            User currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            ViewData["error"] = "";
            Job job = _applicationDbContext.Jobs.Where(a=>a.Id==id).Include(a=>a.Applicants).First();
            if (!IsWorker()||job==null)
            {
                return Redirect("/");
            }
            ViewData["NumberOfApplicants"] = job.Applicants.Count;
            ViewData["JobName"] = job.Name;
            if (job.Applicants.Any(a => a.WorkerAccountId == currentUser.WorkerAccountId))
            {
                ViewData["error"] = "You have already applied for this job!";
                return View();
            }
            if (string.IsNullOrEmpty(JobApplicationMessage??"".Trim()))
            {
                ViewData["error"] = "Job Application cannot be empty!";
                return View();
            }
            WorkerAccountApplication workerAccountApplication = new WorkerAccountApplication()
            {
                ApplicationMessage = JobApplicationMessage,
                JobId = job.Id,
                WorkerAccountId = (int)currentUser.WorkerAccountId
            };
            _applicationDbContext.WorkerAccountApplications.Add(workerAccountApplication);
            _applicationDbContext.SaveChanges();
            ViewData["Success"] = "Application sent successfully!";
            return View();
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