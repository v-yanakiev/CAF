using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChoresAndFulfillment.Data
{
    public class User:IdentityUser
    {
        public int? EmployerAccountId { get; set; }
        public EmployerAccount EmployerAccount { get; set; }
        public int? WorkerAccountId { get; set; }
        public WorkerAccount WorkerAccount { get; set; }
        public List<Message> MessagesSent { get; set; }
        public List<Message> MessagesReceived { get; set; }
        public List<Rating> RatingsReceived { get; set; }
        public double Rating { get => this.RatingsReceived.Average(a => a.Value); }
        public List<Rating> RatingsGiven { get; set; }
        public string AccountType
        {
            get
            {
                if (this.EmployerAccountId != null)
                {
                    return "EmployerAccount";
                }
                else
                {
                    return "WorkerAccount";
                }
            }
        }
    }
}
