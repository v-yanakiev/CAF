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
    public class YourApplicationsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly CAFContext _applicationDbContext;
        public YourApplicationsController(UserManager<User> userManager, CAFContext applicationDbContext)
        {
            this._userManager = userManager;
            this._applicationDbContext = applicationDbContext;
        }
        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            if (!IsWorker())
            {
                return Redirect("/");
            }
            User user = this._userManager.GetUserAsync(HttpContext.User).Result;
            WorkerAccount workerAccount = _applicationDbContext.
                WorkerAccounts.Where(a => a.Id == user.WorkerAccountId).
                Include(a => a.ActiveApplications).ThenInclude(a=>a.Job).First();
            YourApplicationsViewModel model = new YourApplicationsViewModel();
            model.AnyActiveApplications= workerAccount.ActiveApplications.Any();
            if (!model.AnyActiveApplications)
            {
                return View(model);
            }
            model.ActiveApplications = workerAccount.ActiveApplications;
            
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