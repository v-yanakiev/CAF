using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChoresAndFulfillment.Data;
using ChoresAndFulfillment.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using ChoresAndFulfillment.Models;
using ChoresAndFulfillment.Web.Services.Interfaces;
using ChoresAndFulfillment.Web.Data.ViewModels;

namespace ChoresAndFulfillment.Controllers
{
    public class ListAllJobsController : Controller
    {
        private IListAllJobsService service;

        public ListAllJobsController(IListAllJobsService service)
        {
            this.service = service;
        }

        public IActionResult Index(int page)
        {
            ListAllJobsViewModel model = new ListAllJobsViewModel();
            model.AnyJobs = service.AnyActiveJobs();
            if (!model.AnyJobs)
            {
                return this.View(model);
            }
            if (page == 0)
            {
                page = 1;
            }
            if (service.PageIsOutOfBounds(page))
            {
                return Redirect("/ListAllJobs");
            }
            model.IsEmployer = service.IsEmployer();
            model.IsWorker = service.IsWorker();
            model.ActiveJobs = service.ViewAllActiveJobs(page);
            model.PageIsLast = service.PageIsLast(page);
            if (model.IsWorker)
            {
                foreach (var job in model.ActiveJobs)
                {
                    if (service.WorkerAlreadyApplied(job.Id))
                    {
                        job.CurrentUserAppliedForJob = true;
                    }
                    else
                    {
                        job.CurrentUserAppliedForJob = false;
                    }
                }
            }
            return this.View(model);
        }

    }
}