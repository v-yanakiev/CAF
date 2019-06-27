using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChoresAndFulfillment.Models;

namespace ChoresAndFulfillment.Web.Data.ViewModels
{
    public class YourActiveJobsViewModel
    {
        public int MyProperty { get; set; }
        public bool AnyActiveJobs { get; set; }
        public IEnumerable<Job> ActiveJobs { get; set; }
    }
}
