using ChoresAndFulfillment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChoresAndFulfillment.Web.Data.ViewModels
{
    public class ActiveJobViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string JobCreatorName { get; set; }
        public double? JobCreatorRating { get; set; }
        public decimal PayUponCompletion { get; set; }
        public bool CurrentUserAppliedForJob { get; set; }
    }
}
