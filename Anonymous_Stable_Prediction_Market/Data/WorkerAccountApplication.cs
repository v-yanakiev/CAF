using ChoresAndFulfillment.Data.Enums;

namespace ChoresAndFulfillment.Data
{
    public class WorkerAccountApplication
    {
        public WorkerAccountApplication()
        {
            this.ApplicationState = ApplicationState.Pending;
        }
        public int WorkerAccountId { get; set; }
        public WorkerAccount WorkerAccount { get; set; }
        public int JobId { get; set; }
        public Job Job { get; set; }
        public string ApplicationMessage { get; set; }
        public ApplicationState ApplicationState { get; set; }
    }
}