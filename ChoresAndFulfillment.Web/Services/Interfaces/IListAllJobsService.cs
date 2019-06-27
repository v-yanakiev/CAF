using ChoresAndFulfillment.Models;
using ChoresAndFulfillment.Web.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChoresAndFulfillment.Web.Services.Interfaces
{
    public interface IListAllJobsService:
        IUserAndContextRepository
    {
        bool AnyActiveJobs();
        List<ActiveJobViewModel> ViewAllActiveJobs(int page);
        bool WorkerAlreadyApplied(int jobId);
        bool PageIsLast(int pageNumber);
        bool PageIsOutOfBounds(int pageNumber);
    }
}
