using System.ComponentModel.DataAnnotations;

namespace ChoresAndFulfillment.Data
{
    public class Rating
    { 
        public int Id { get; set; }
        [Range(1,5)]
        public int Value { get; set; }
        public int JobAndWorkerId { get; set; }
        public WorkerAccountApplication JobAndWorker { get; set; }
    }
}