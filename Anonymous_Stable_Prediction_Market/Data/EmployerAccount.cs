using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ChoresAndFulfillment.Data
{
    public class EmployerAccount
    {
        public EmployerAccount()
        {
            AccountBalance = 0;
        }

        public int Id { get; set; }
        public decimal AccountBalance { get; set; }
        //[ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
        public List<Job> CreatedJobs { get; set; }
        
    }
}