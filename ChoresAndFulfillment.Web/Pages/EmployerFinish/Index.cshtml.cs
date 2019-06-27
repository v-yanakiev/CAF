using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChoresAndFulfillment.Data;
using ChoresAndFulfillment.Models;
using ChoresAndFulfillment.Models.Enums;
using ChoresAndFulfillment.Web.Data.BindModels;
using ChoresAndFulfillment.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChoresAndFulfillment.Web.Pages.EmployerFinish
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly CAFContext _applicationDbContext;

        public IndexModel(UserManager<User> userManager, CAFContext applicationDbContext)
        {
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
        }
        [Authorize]
        public IActionResult OnGet(int id)
        {
            if (IsWorker() || !IsValidJob(id))
            {
                return Redirect("/");
            }
            ViewData["JobId"] = id;
            return Page();
        }
        
        public IActionResult MarkAsFinished(int jobId,int rating)
        {
            if (IsWorker() || !IsValidJob(jobId))
            {
                return Redirect("/");
            }
            Job job = _applicationDbContext.Jobs.FirstOrDefault(a => a.Id == jobId);
            job.EmployerConfirmedFinished = true;
            _applicationDbContext.SaveChanges();
            return Redirect("/ManageJob/Manage/" + jobId);
        }
        private bool IsValidJob(int id)
        {
            Job job = _applicationDbContext.Jobs.FirstOrDefault(a => a.Id == id && (a.JobState == JobState.Hiring || a.JobState == JobState.Hired));
            User user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (job != null)
            {
                if (user.EmployerAccountId == job.JobCreatorId)
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