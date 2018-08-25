using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChoresAndFulfillment.Data;
using ChoresAndFulfillment.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChoresAndFulfillment.Controllers
{
    public class CreateJobController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _applicationDbContext;
        public CreateJobController(UserManager<User> userManager, ApplicationDbContext applicationDbContext)
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
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult Index(CreateJobViewModel cjvm)
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User);
            if (IsWorker())
            {
                return Redirect("/WorkerManagement/Index");
            }
            if (cjvm.Payment == 0)
            {
                ViewData["Error"] = "Invalid payment!";
                return View();
            }
            string JobName = cjvm.JobName;
            string Description = cjvm.Description;
            decimal Payment = cjvm.Payment;
            int accountId = (int)currentUser.Result.EmployerAccountId;
            //EmployerAccount employerAccount = _applicationDbContext.
            //    EmployerAccounts.First(a => a.Id == accountId);
            //employerAccount.UserId = currentUser.Result.Id;
            Job job = new Job
            {
                Name = JobName,
                Description = Description,
                PayUponCompletion = Payment,
                JobCreatorId=(int)currentUser.Result.EmployerAccountId
            };
            _applicationDbContext.Jobs.Add(job);
            _applicationDbContext.SaveChanges();
            ViewData["Success"] = "Published job successfully!";
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