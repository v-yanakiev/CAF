using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChoresAndFulfillment.Web.Services.Interfaces
{
    public interface IApplyForJobService:IUserAndContextRepository
    {
        bool AlreadyAppliedForJob(int jobId);
        void SignUpWorkerForJob(int? workerAccountId, int jobId, string message);
    }
}
