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
    public class YourApplicationsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _applicationDbContext;
        public YourApplicationsController(UserManager<User> userManager, ApplicationDbContext applicationDbContext)
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
            if (!workerAccount.ActiveApplications.Any())
            {
                ViewData["applications"] = "<h2>You haven't made any applications yet!</h2>";
                return View();
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<table class=\"table\"\">");
            stringBuilder.AppendLine("<thead>");
            stringBuilder.AppendLine("<tr>");
            stringBuilder.AppendLine("<th scope=\"col\">Name of job</th>");
            stringBuilder.AppendLine("<th scope=\"col\">Description</th>");
            stringBuilder.AppendLine("<th scope=\"col\">Payment</th>");
            stringBuilder.AppendLine("<th scope=\"col\">Status</th>");
            stringBuilder.AppendLine("</tr>");
            stringBuilder.AppendLine("</thead>");
            stringBuilder.AppendLine("<tbody>");
            foreach (var application in workerAccount.ActiveApplications)
            {
                stringBuilder.AppendLine("<tr>");
                stringBuilder.AppendLine("<td>" + application.Job.Name + "</td>");
                stringBuilder.AppendLine("<td>" + application.Job.Description + "</td>");
                stringBuilder.AppendLine("<td>" + application.Job.PayUponCompletion + "</td>");
                stringBuilder.AppendLine("<td>" + this.ParseApplicationState(application.ApplicationState) + "</td>");
                stringBuilder.AppendLine("</tr>");

            }
            stringBuilder.AppendLine("</tbody>");
            stringBuilder.AppendLine("</table>");
            ViewData["applications"] = stringBuilder.ToString();
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
        private string ParseApplicationState(ApplicationState applicationState)
        {
            if (applicationState == ApplicationState.Deleted)
            {
                return "Job has been deleted.";
            }
            else
            {
                return applicationState.ToString();
            }
        }
    }
}