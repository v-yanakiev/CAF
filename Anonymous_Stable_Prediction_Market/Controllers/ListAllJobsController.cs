using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChoresAndFulfillment.Data;
using ChoresAndFulfillment.Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChoresAndFulfillment.Controllers
{
    public class ListAllJobsController : Controller
    {
        private ApplicationDbContext applicationDbContext;
        private UserManager<User> userManager;
        public ListAllJobsController(
            ApplicationDbContext applicationDbContext, 
            UserManager<User> userManager)
        {
            this.applicationDbContext = applicationDbContext;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            bool isEmployer = false;
            if (User.Identity.IsAuthenticated)
            {
                User currentUser = userManager.GetUserAsync(HttpContext.User).Result;
                if (currentUser.EmployerAccountId != null)
                {
                    isEmployer = true;
                }
            }
            if (!applicationDbContext.Jobs.Any(a=>a.JobState==JobState.Active))
            {
                ViewData["jobs"] = "<h1 style=\"text-align:center;\">No jobs found!</h1>";
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("<table class=\"table\"\">");
                stringBuilder.AppendLine("<thead>");
                stringBuilder.AppendLine("<tr>");
                stringBuilder.AppendLine("<th scope=\"col\">Job Name</th>");
                stringBuilder.AppendLine("<th scope=\"col\">Description</th>");
                stringBuilder.AppendLine("<th scope=\"col\">Job Creator</th>");
                stringBuilder.AppendLine("<th scope=\"col\">Employer Rating</th>");
                stringBuilder.AppendLine("<th scope=\"col\">Payment</th>");
                if (!isEmployer)
                {
                    stringBuilder.AppendLine("<th scope=\"col\">Apply for Job</th>");
                }
                stringBuilder.AppendLine("</tr>");
                stringBuilder.AppendLine("</thead>");
                stringBuilder.AppendLine("<tbody>");

                foreach (var job in applicationDbContext.Jobs.Include(job=>job.JobCreator).
                    ThenInclude(jobCreator=>jobCreator.User).ThenInclude(user=>user.RatingsReceived).Where(job=> job.JobState == JobState.Active))
                {
                    stringBuilder.AppendLine("<tr>");
                    stringBuilder.AppendLine("<td>"+job.Name+"</td>");
                    stringBuilder.AppendLine("<td>" + job.Description + "</td>");
                    stringBuilder.AppendLine("<td>" + job.JobCreator.User.UserName + "</td>");
                    if (job.JobCreator.User.RatingsReceived.Any())
                    {
                        stringBuilder.AppendLine("<td>" + job.JobCreator.User.Rating + "</td>");
                    }
                    else
                    {
                        stringBuilder.AppendLine("<td>N/A</td>");
                    }
                    stringBuilder.AppendLine("<td>" + job.PayUponCompletion+ "</td>");

                    if (!isEmployer)
                    {
                        stringBuilder.AppendLine($"<td><a href=\"/ApplyForJob/Apply/{job.Id}\">Apply For Job</a></td>");
                    }
                    stringBuilder.AppendLine("</tr>");
                }
                stringBuilder.AppendLine("</tbody>");
                stringBuilder.AppendLine("</table>");
                ViewData["jobs"] = stringBuilder.ToString();
            }
            return this.View();
        }
    }
}