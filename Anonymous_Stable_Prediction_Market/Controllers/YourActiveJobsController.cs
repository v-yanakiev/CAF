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
    public class YourActiveJobsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _applicationDbContext;
        public YourActiveJobsController(UserManager<User> userManager, ApplicationDbContext applicationDbContext)
        {
            this._userManager = userManager;
            this._applicationDbContext = applicationDbContext;
        }
        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            if (IsWorker())
            {
                return Redirect("/WorkerManagement/Index");
            }
            User user = this._userManager.GetUserAsync(HttpContext.User).Result;
            EmployerAccount employerAccount =
                _applicationDbContext.
                EmployerAccounts.Where(a => a.Id == user.EmployerAccountId).
                Include(a => a.CreatedJobs).First();
            if (!employerAccount.CreatedJobs.Any(a => a.JobState==JobState.Active))
            {
                ViewData["Jobs"] = "<h2>You have no active jobs!</h2>";
                return View();
            }
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var job in employerAccount.CreatedJobs.Where(a => a.JobState == JobState.Active))
            {
                stringBuilder.AppendLine("<li>");
                stringBuilder.AppendLine("<a href=\"/ManageJob/Manage/" + job.Id + "\">" + job.Name + "</a>");
                stringBuilder.AppendLine("</li>");
            }
            ViewData["Jobs"] = stringBuilder.ToString();
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