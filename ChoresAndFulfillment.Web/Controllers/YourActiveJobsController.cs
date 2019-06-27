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
    public class YourActiveJobsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly CAFContext _applicationDbContext;
        public YourActiveJobsController(UserManager<User> userManager, CAFContext applicationDbContext)
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
            YourActiveJobsViewModel model = new YourActiveJobsViewModel();
            model.AnyActiveJobs = 
                employerAccount.CreatedJobs.Any(a => a.JobState == JobState.Hiring || a.JobState == JobState.Hired);
            if (!model.AnyActiveJobs)
            {
                return View(model);
            }
            model.ActiveJobs = employerAccount.CreatedJobs.Where(a => a.JobState == JobState.Hiring || a.JobState == JobState.Hired);
            
            return View(model);
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