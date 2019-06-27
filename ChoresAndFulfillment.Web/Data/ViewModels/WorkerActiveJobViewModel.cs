using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChoresAndFulfillment.Web.Data.ViewModels
{
    public class WorkerActiveJobViewModel
    {
        public bool WorkerConfirmedFinished { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; }
        public string JobDescription { get; set; }
        public string JobCreator { get; set; }
        public string PayUponCompletion { get; set; }
    }
}
