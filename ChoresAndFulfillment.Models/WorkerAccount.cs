using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ChoresAndFulfillment.Models
{
    public class WorkerAccount
    {
        public WorkerAccount()
        {
            AccountBalance = 0;
            Jobs = new List<Job>();
            ActiveApplications = new List<WorkerAccountApplication>();
        }

        public int Id { get; set; }
        public decimal AccountBalance { get; set; }
        //[ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
        public List<WorkerAccountApplication> ActiveApplications { get; set; }
        public List<Job> Jobs { get; set; }
    }
}