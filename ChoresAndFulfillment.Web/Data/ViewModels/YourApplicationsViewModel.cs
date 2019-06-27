using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChoresAndFulfillment.Models;

namespace ChoresAndFulfillment.Web.Data.ViewModels
{
    public class YourApplicationsViewModel
    {
        public bool AnyActiveApplications { get; set; }
        public List<WorkerAccountApplication> ActiveApplications { get; set; }
    }
}
