using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChoresAndFulfillment.Data;
using ChoresAndFulfillment.Models;
using ChoresAndFulfillment.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChoresAndFulfillment.Web.Pages.EmployerFinish
{
    public class FinishModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly CAFContext _applicationDbContext;

        [Authorize]
        public IActionResult OnGet([BindRequired]int Rating, [BindRequired]int JobId)
        {
            if (IsWorker() || !IsValidJob(JobId))
            {
                return Redirect("/");
            }
            if (Rating != 1 && Rating != 2 && Rating != 3 && Rating != 4 && Rating != 5)
            {
                return Redirect("/");
            }
            //MarkAsFinished(ratingGivenModel.JobId,ratingGivenModel.Rating);
            return Redirect("/ListAllJobs");
            //return Page();
        }
        public IActionResult MarkAsFinished(int jobId, int rating)
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