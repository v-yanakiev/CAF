using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChoresAndFulfillment.Models;

namespace ChoresAndFulfillment.Web.Data.ViewModels
{
    public class ManageJobViewModel
    {
        public bool AnyApplications { get; set; }
        public bool AcceptedAnyone { get; set; }
        public WorkerAccountApplication AcceptedApplication { get; set; }
        public Job Job { get; set; }
        public IEnumerable<WorkerAccountApplication> PendingApplications { get; set; }
    }
}
