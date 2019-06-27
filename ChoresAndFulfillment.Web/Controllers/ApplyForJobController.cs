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
using ChoresAndFulfillment.Web.Services.Interfaces;
using ChoresAndFulfillment.Web.Data.BindModels;

namespace ChoresAndFulfillment.Controllers
{
    public class ApplyForJobController : Controller
    {
        IApplyForJobService service;

        public ApplyForJobController(IApplyForJobService service)
        {
            this.service = service;
        }
        [Authorize]
        [HttpGet]
        public IActionResult Apply(int id)
        {
            ViewData["error"] = "";
            if ((!service.IsWorker())||(!service.JobExists(id)))
            {
                return Redirect("/");
            }
            Job job = service.GetJob(id);
            ViewData["JobName"] = job.Name;
            ViewData["NumberOfApplicants"] = job.Applicants.Count;
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult Apply(ApplyForJobBindModel applyForJobBindModel)
        {
            ViewData["error"] = "";
            if ((!service.IsWorker()) || (!service.JobExists(applyForJobBindModel.Id)))
            {
                return Redirect("/");
            }
            Job job = service.GetJob(applyForJobBindModel.Id);
            if (service.AlreadyAppliedForJob(applyForJobBindModel.Id))
            {
                ViewData["error"] = "You have already applied for this job!";
                return View();
            }
            if (string.IsNullOrEmpty(applyForJobBindModel.JobApplicationMessage??"".Trim()))
            {
                ViewData["error"] = "Job Application must be at least 5 symbols!";
                return View();
            }
            User currentUser = service.GetCurrentUser();
            service.SignUpWorkerForJob
                (currentUser.WorkerAccountId,job.Id,applyForJobBindModel.JobApplicationMessage);
            ViewData["Success"] = "Application sent successfully!";
            ViewData["NumberOfApplicants"] = job.Applicants.Count;
            ViewData["JobName"] = job.Name;
            return View();
        }
    }
}