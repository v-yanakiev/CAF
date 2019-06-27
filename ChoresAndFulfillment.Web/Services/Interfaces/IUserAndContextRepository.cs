using ChoresAndFulfillment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChoresAndFulfillment.Web.Services.Interfaces
{
    public interface IUserAndContextRepository
    {
        User GetCurrentUser();
        Job GetJob(int jobId);
        void AddJob(Job job);
        bool IsAuthenticated();
        bool IsEmployer();
        bool IsWorker();
        bool JobExists(int jobId);
    }
}
