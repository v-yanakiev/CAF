using ChoresAndFulfillment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChoresAndFulfillment.Web.Data.ViewModels
{
    public class ListAllJobsViewModel
    {
        public bool AnyJobs { get; set; }
        public List<ActiveJobViewModel> ActiveJobs { get; set; }
        public int CurrentPage { get; set; }
        public bool PageIsLast { get; set; }
        public bool IsEmployer { get; set; }
        public bool IsWorker { get; set; }
    }
}
