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
using ChoresAndFulfillment.Web.Services.Interfaces;
using ChoresAndFulfillment.Web.Data.ViewModels;

namespace ChoresAndFulfillment.Controllers
{
    public class ListWorkerJobsController : Controller
    {
        private IListWorkerJobsService service;

        public ListWorkerJobsController(IListWorkerJobsService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            ListWorkerJobsViewModel model = new ListWorkerJobsViewModel();
            if (!service.IsWorker())
            {
                return Redirect("/");
            }
            User user = service.GetCurrentUser();
            WorkerAccount workerAccount = service.GetWorkerAccount();
            model.AnyActiveJobs = service.HasActiveJobs();
            if (!model.AnyActiveJobs)
            {
                return View(model);
            }
            model.ActiveJobs = service.ActiveJobs();
            return View(model);
        }
    }
}