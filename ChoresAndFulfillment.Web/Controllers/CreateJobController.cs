using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChoresAndFulfillment.Data;
using ChoresAndFulfillment.Data.BindModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


using ChoresAndFulfillment.Models;
using ChoresAndFulfillment.Web.Services.Interfaces;

namespace ChoresAndFulfillment.Controllers
{
    public class CreateJobController : Controller
    {
        IUserAndContextRepository repository;
        public CreateJobController(IUserAndContextRepository repository)
        {
            this.repository = repository;
        }
        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            if (repository.IsWorker())
            {
                ViewData["Error"]= "You are a worker, not an employer!";
                return Redirect("/WorkerManagement/Index");
            }
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult Index(CreateJobBindModel cjvm)
        {
            var currentUser = repository.GetCurrentUser();
            if (repository.IsWorker())
            {
                ViewData["Error"] = "You are a worker!";
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
            int accountId = (int)currentUser.EmployerAccountId;
            Job job = new Job
            {
                Name = JobName,
                Description = Description,
                PayUponCompletion = Payment,
                JobCreatorId=(int)currentUser.EmployerAccountId
            };
            repository.AddJob(job);
            ViewData["Success"] = "Succesfully created job!";
            return View();
        }
    }
}