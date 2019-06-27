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
namespace ChoresAndFulfillment.Controllers
{
    public class WorkerFinishedJobController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly CAFContext _applicationDbContext;
        public WorkerFinishedJobController(UserManager<User> userManager, CAFContext applicationDbContext)
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
            return View();
        }
        [HttpPost]
        [Authorize]
        public IActionResult Index(int id,string rating)
        {
            if (!IsWorker() || !IsValidJob(id))
            {
                return Redirect("/");
            }
            if (!rating.All(a => a >= '1' && a <= '5'))
            {
                ViewData["error"] = "Invalid rating!";
                return View("/");
            }
            int ratingNumber = int.Parse(rating);
            _applicationDbContext.SaveChanges();
            return Redirect("/WorkerFinishedJob/Index/" + id);
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
            Job job = _applicationDbContext.Jobs.FirstOrDefault(a => a.Id == id && a.JobState == JobState.Finished);
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