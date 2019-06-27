using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChoresAndFulfillment.Models;

namespace ChoresAndFulfillment.Web.Data.ViewModels
{
    public class ListWorkerJobsViewModel
    {
        public bool AnyActiveJobs { get; set; }
        public List<Job> ActiveJobs { get; set; }
    }
}
