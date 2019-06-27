using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChoresAndFulfillment.Models
{
    public class User:IdentityUser
    {
        public User()
        {
            MessagesSent = new List<Message>();
            MessagesReceived = new List<Message>();
            RatingsReceived = new List<Rating>();
        }

        public int? EmployerAccountId { get; set; }
        public EmployerAccount EmployerAccount { get; set; }
        public int? WorkerAccountId { get; set; }
        public WorkerAccount WorkerAccount { get; set; }
        public List<Message> MessagesSent { get; set; }
        public List<Message> MessagesReceived { get; set; }
        public List<Rating> RatingsReceived { get; set; }
        public double? Rating {
            get
            {
                if (this.RatingsReceived.Any())
                {
                    return this.RatingsReceived.Average(a => a.Value);
                }
                else
                {
                    return null;
                }
            }
        }
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
