using ChoresAndFulfillment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChoresAndFulfillment.Web.Services.Interfaces
{
    public interface IListWorkerJobsService:IUserAndContextRepository
    {
        WorkerAccount GetWorkerAccount();
        bool HasActiveJobs();
        List<Job> ActiveJobs(); 
    }
}
